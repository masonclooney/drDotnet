using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using drDotnet.Services.Contact.API.Controllers;
using drDotnet.Services.Contact.API.Infrastructure;
using drDotnet.Services.Contact.API.Model;
using drDotnet.Services.Contact.API.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace drDotnet.Services.Contact.UnitTests.Application
{
    public class ContactControllerTest
    {
        private readonly DbContextOptions<ContactContext> _dbOptions;

        public ContactControllerTest()
        {
            _dbOptions = new DbContextOptionsBuilder<ContactContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using(var dbContext = new ContactContext(_dbOptions))
            {
                dbContext.AddRange(GetFakeUsers());
                dbContext.AddRange(GetFakeContacts());
                dbContext.SaveChanges();
            }
        }

        [Fact]
        public async Task Delete_contact_nocontent()
        {
            // Arrange
            var contactContext = new ContactContext(_dbOptions);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1")
            }, "mock"));

            // Act
            var contactController = new ContactController(contactContext);
            contactController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            var actionResult = await contactController.DeleteContactAsync(2);

            // Assert
            Assert.IsType<NoContentResult>(actionResult);
        }

        [Fact]
        public async Task Delete_contact_notfound()
        {
            // Arrange
            var contactContext = new ContactContext(_dbOptions);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1")
            }, "mock"));

            // Act
            var contactController = new ContactController(contactContext);
            contactController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            var actionResult = await contactController.DeleteContactAsync(4);

            // Assert
            Assert.IsType<NotFoundResult>(actionResult);
        }

        [Fact]
        public async Task Get_contacts_success()
        {
            // Arrange
            var pageSize = 4;
            var pageIndex = 0;

            var expectedItemsInPage = 2;
            var expectedTotalItems = 2;

            var contactContext = new ContactContext(_dbOptions);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1")
            }, "mock"));

            // Act
            var contactController = new ContactController(contactContext);
            contactController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            var actionResult = await contactController.ItemsAsync(pageSize, pageIndex);

            // Assert
            Assert.IsType<ActionResult<PaginatedItemsViewModel<User>>>(actionResult);
            var page = Assert.IsAssignableFrom<PaginatedItemsViewModel<User>>(actionResult.Value);
            Assert.Equal(expectedTotalItems, page.Count);
            Assert.Equal(pageIndex, page.PageIndex);
            Assert.Equal(pageSize, page.PageSize);
            Assert.Equal(expectedItemsInPage, page.Data.Count());
        }


        [Fact]
        public async Task Create_contact_success()
        {
            // Arrange
            var contactContext = new ContactContext(_dbOptions);
            var contact = new ContactCreateDto
            {
                Name = "safari",
                Email = "mohsen@gmail.com"
            };
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1")
            }, "mock"));

            // Act
            var contactController = new ContactController(contactContext);
            contactController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            var actionResult = await contactController.CreateContactAsync(contact);

            //Assert
            var result = Assert.IsType<CreatedResult>(actionResult);
            var data = Assert.IsAssignableFrom<API.Model.Contact>(result.Value);
            Assert.Equal("safari", data.Name);
            Assert.Equal(4, data.UserId);
            Assert.Equal(1, data.OwnerId);
        }

        [Fact]
        public async Task Create_contact_update()
        {
            // Arrange
            var contactContext = new ContactContext(_dbOptions);
            var contact = new ContactCreateDto
            {
                Name = "lida afshari",
                Email = "lida@gmail.com"
            };
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1")
            }, "mock"));

            // Act
            var contactController = new ContactController(contactContext);
            contactController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            var actionResult = await contactController.CreateContactAsync(contact);

            //Assert
            var result = Assert.IsType<OkObjectResult>(actionResult);
            var data = Assert.IsAssignableFrom<API.Model.Contact>(result.Value);
            Assert.Equal("lida afshari", data.Name);
            Assert.Equal(3, data.UserId);
            Assert.Equal(1, data.OwnerId);
        }

        [Fact]
        public async Task Create_contact_notfound()
        {
            // Arrange
            var contactContext = new ContactContext(_dbOptions);
            var contact = new ContactCreateDto
            {
                Name = "hossein naser",
                Email = "hosein@gmail.com"
            };
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1")
            }, "mock"));

            // Act
            var contactController = new ContactController(contactContext);
            contactController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            var actionResult = await contactController.CreateContactAsync(contact);

            //Assert
            Assert.IsType<NotFoundResult>(actionResult);
        }

        private List<API.Model.Contact> GetFakeContacts()
        {
            return new List<API.Model.Contact>
            {
                new API.Model.Contact
                {
                    OwnerId = 1,
                    UserId = 2,
                    Name = "mohammad"
                },
                new API.Model.Contact
                {
                    OwnerId = 1,
                    UserId = 3,
                    Name = "lida"
                }
            };
        }

        private List<API.Model.User> GetFakeUsers()
        {
            return new List<API.Model.User>
            {
                new API.Model.User
                {
                    Id = 1,
                    Email = "kholusi95@gmail.com",
                    Name = "meysam"
                },
                new API.Model.User
                {
                    Id = 2,
                    Email = "mmd@gmail.com",
                    Name = "mmd"
                },
                new API.Model.User
                {
                    Id = 3,
                    Email = "lida@gmail.com",
                    Name = "lida"
                },
                new API.Model.User
                {
                    Id = 4,
                    Email = "mohsen@gmail.com",
                    Name = "mohsen"
                },
            };
        }
    }
}