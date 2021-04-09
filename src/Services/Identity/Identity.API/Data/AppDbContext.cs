using drDotnet.Services.Identity.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace drDotnet.Services.Identity.API.Data
{
    public class AppDbContext : IdentityDbContext<AppIdentityUser, IdentityRole<long>, long>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}