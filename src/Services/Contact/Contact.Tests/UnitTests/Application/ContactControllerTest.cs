using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using drDotnet.Services.Contact.API.Controllers;
using drDotnet.Services.Contact.API.Infrastructure;
using drDotnet.Services.Contact.API.ViewModel;
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
        public async Task Create_contact_success()
        {
            // Arrange
            var contactContext = new ContactContext(_dbOptions);
            var contact = new ContactCreateDto
            {
                Name = "safari",
                Email = "mohsen@gmail.com"
            };

            // Act
            var contactController = new ContactController(contactContext);
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

            // Act
            var contactController = new ContactController(contactContext);
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

            // Act
            var contactController = new ContactController(contactContext);
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