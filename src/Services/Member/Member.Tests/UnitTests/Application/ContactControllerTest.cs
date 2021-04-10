using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using drDotnet.Services.Member.API.Controllers;
using drDotnet.Services.Member.API.Infrastructure;
using drDotnet.Services.Member.API.Model;
using drDotnet.Services.Member.API.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace drDotnet.Services.Member.UnitTests.Application
{
    public class ContactControllerTest
    {
        private readonly DbContextOptions<MemberContext> _dbOptions;

        public ContactControllerTest()
        {
            _dbOptions = new DbContextOptionsBuilder<MemberContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using(var dbContext = new MemberContext(_dbOptions))
            {
                dbContext.AddRange(GetFakeUser());
                dbContext.AddRange(GetFakeContact());
                dbContext.SaveChanges();
            }
        }

        [Fact]
        public async Task Create_contact_success()
        {
            // Arange
            var memberContext = new MemberContext(_dbOptions);
            var user = new User
            {
                Email = "mmd@gmail.com",
                Name = "mmd"
            };

            // Act
            var contactController = new ContactController(memberContext);
            var actionResult = await contactController.CreateContactAsync(user);

            // Assert
            Assert.IsType<CreatedAtActionResult>(actionResult);
        }

        [Fact]
        public async Task Get_contact_by_id_success()
        {
            // Arange
            var memberContext = new MemberContext(_dbOptions);

            // Act
            var contactController = new ContactController(memberContext);
            var actionResult = await contactController.ItemByIdAsync(1);

            // Assert
            var result = Assert.IsType<ActionResult<ContactViewModel>>(actionResult);
            var data = Assert.IsAssignableFrom<ContactViewModel>(result.Value);
            Assert.Equal("mmd", data.Name);
            Assert.Equal(2, data.UserId);
            Assert.Equal("mmd@gmail.com", data.Email);
        }

        [Fact]
        public async Task Get_contact_items_success()
        {
            // Arrange
            var pageSize = 4;
            var pageIndex = 0;

            var expectedItemsInPage = 2;
            var expectedTotalItems = 2;

            var memberContext = new MemberContext(_dbOptions);

            // Act
            var contactController = new ContactController(memberContext);
            var actionResult = await contactController.ItemsAsync(pageSize, pageIndex);

            // Assert
            var result = Assert.IsType<OkObjectResult>(actionResult);
            var page = Assert.IsAssignableFrom<PaginatedItemsViewModel<ContactViewModel>>(result.Value);
            Assert.Equal(expectedTotalItems, page.Count);
            Assert.Equal(pageIndex, page.PageIndex);
            Assert.Equal(pageSize, page.PageSize);
            Assert.Equal(expectedItemsInPage, page.Data.Count());
        }

        private List<Contact> GetFakeContact()
        {
            return new List<Contact>()
            {
                new Contact()
                {
                    Id = 1,
                    Name = "mmd",
                    OwnerId = 1,
                    UserId = 2
                },
                new Contact()
                {
                    Id = 2,
                    Name = "meysam",
                    OwnerId = 2,
                    UserId = 1
                }
            };
        }

        private List<User> GetFakeUser()
        {
            return new List<User>()
            {
                new User()
                {
                    Id = 1,
                    Email = "kholusi95@gmail.com",
                    Sub = "asd0f98-asdf-08-asdf0897",
                    Name = "meysam"
                },
                new User()
                {
                    Id = 2,
                    Email = "mmd@gmail.com",
                    Name = "mmd",
                    Sub = "fgas-9-a"
                }
            };
        }
    }
}