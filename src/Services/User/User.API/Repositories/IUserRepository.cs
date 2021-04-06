using System.Collections.Generic;
using System.Threading.Tasks;
using drDotnet.Services.User.API.Models;

namespace drDotnet.Services.User.API.Repositories
{
    public interface IUserRepository
    {
        UserModel CreateUser(UserModel user);

        List<UserModel> GetUsers(List<long> ids);

        UserModel GetByEmail(string email);

        UserModel GetByPhone(string phone);

        UserModel GetbyPhoneAndEmail(string phone, string email);
    }
}