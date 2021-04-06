using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using drDotnet.Services.User.API.Models;

namespace drDotnet.Services.User.API.Repositories
{
    public class UserRepository : IUserRepository
    {

        private static long id = 977588815;

        private static readonly IDictionary<long, UserModel> Users =
            new Dictionary<long, UserModel>
            {
                { (id - 1), new UserModel { Id = id - 1 , Email = "kholoosim1@gmail.com", FirstName = "meysam", LastName = "kholousi", PhoneNumber = "09054641341", Sub = Guid.NewGuid().ToString() } },
                { (id - 2), new UserModel { Id = id - 2 , Email = "lidaafshari@gmail.com", FirstName = "lida", LastName = "fashari", PhoneNumber = "09032650380", Sub = Guid.NewGuid().ToString() } },
                { (id - 3), new UserModel { Id = id - 3 , Email = "mohammadjavad@gamil.com", FirstName = "mohammad", LastName = "akbari", PhoneNumber = "09022001822", Sub = Guid.NewGuid().ToString() } },
                { (id - 4), new UserModel { Id = id - 4 , Email = "mohsensafari@gmail.com", FirstName = "mohsen", LastName = "safari", PhoneNumber = "09398800380", Sub = Guid.NewGuid().ToString() } },
                { (id - 5), new UserModel { Id = id - 5 , Email = "navidsadeghpoor@gmail.com", FirstName = "navid", LastName = "sadeghpoor", PhoneNumber = "09603265987", Sub = Guid.NewGuid().ToString() } },
                { (id - 6), new UserModel { Id = id - 6 , Email = "rasoulkhalife@gmail.com", FirstName = "rasoul", LastName = "khalife", PhoneNumber = "09065658424", Sub = Guid.NewGuid().ToString() } },
                { (id - 7), new UserModel { Id = id - 7 , Email = "mehdinaghavi@gmail.com", FirstName = "mehdi", LastName = "naghavi", PhoneNumber = "09054647414", Sub = Guid.NewGuid().ToString() } },
                { (id - 8), new UserModel { Id = id - 8 , Email = "alirezahadipur@gmail.com", FirstName = "alireza", LastName = "hadipur", PhoneNumber = "09056569850", Sub = Guid.NewGuid().ToString() } },
                { (id - 9), new UserModel { Id = id - 9 , Email = "khoda@gmail.com", FirstName = "khoda", LastName = "khalegh", PhoneNumber = "09122222222", Sub = Guid.NewGuid().ToString() } },
            };

        public UserModel CreateUser(UserModel user)
        {
            long temp = id;
            id++;
            Users.Add(temp, user);
            user.Id = temp;
            return user;
        }

        public List<UserModel> GetUsers(List<long> ids)
        {
            return Users.Where(x => ids.Contains(x.Key)).Select(x => x.Value).ToList();
        }

        public UserModel GetByEmail(string email)
        {
            return Users.Where(x => x.Value.Email == email).Select(x => x.Value).SingleOrDefault();
        }

        public UserModel GetByPhone(string phone)
        {
            return Users.Where(x => x.Value.PhoneNumber == phone).Select(x => x.Value).SingleOrDefault();
        }

        public UserModel GetbyPhoneAndEmail(string phone, string email)
        {
            return Users.Where(x => x.Value.PhoneNumber == phone && x.Value.Email == email).Select(x => x.Value).SingleOrDefault();
        }
    }
}