using System.Linq;
using System.Threading.Tasks;
using drDotnet.Services.User.API.Models;
using drDotnet.Services.User.API.Repositories;
using Grpc.Core;

namespace drDotnet.Services.User.API.Services
{
    public class UserService : UserManager.UserManagerBase
    {

        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public override Task<CreatedUser> CreateUser(CreateUserMessage request, ServerCallContext context)
        {
            var user = new UserModel
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Sub = request.Sub,
            };
            var result = _userRepository.CreateUser(user);
            return Task.FromResult(new CreatedUser
            {
                Id = result.Id,
                Email = result.Email,
                FirstName = result.FirstName,
                LastName = result.LastName,
                Sub = result.Sub,
            });
        }

        public override Task<ExistsResult> EmailExists(Email request, ServerCallContext context)
        {
            var user = _userRepository.GetByEmail(request.Email_);
            if (user != null)
            {
                return Task.FromResult(new ExistsResult
                {
                    Exists = true,
                    Id = user.Id
                });
            }

            return Task.FromResult(new ExistsResult
            {
                Exists = false
            });
        }

        public override Task<ExistsResult> EmailPhoneExists(PhoneEmail request, ServerCallContext context)
        {
            var user = _userRepository.GetbyPhoneAndEmail(request.Phone, request.Email);
            if (user != null)
            {
                return Task.FromResult(new ExistsResult
                {
                    Exists = true,
                    Id = user.Id
                });
            }

            return Task.FromResult(new ExistsResult
            {
                Exists = false
            });
        }

        public override async Task GetUsers(UserIds request, IServerStreamWriter<User> responseStream, ServerCallContext context)
        {
            var users = _userRepository.GetUsers(request.Ids.ToList());
            foreach (var user in users)
            {
                await responseStream.WriteAsync(new User
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    Id = user.Id,
                    LastName = user.LastName,
                    Phone = user.PhoneNumber,
                    Sub = user.Sub
                });
            }
        }

        public override Task<ExistsResult> PhoneExists(Phone request, ServerCallContext context)
        {
            var user = _userRepository.GetByPhone(request.Phone_);
            if (user != null)
            {
                return Task.FromResult(new ExistsResult
                {
                    Exists = true,
                    Id = user.Id
                });
            }

            return Task.FromResult(new ExistsResult
            {
                Exists = false
            });
        }
    }
}