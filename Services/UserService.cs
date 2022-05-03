using System.Linq;
using Models;
using ModelsView;
using ProgramSystem.Bll.Services.Interfaces;
using Repository.factories;
using Repository.UOW;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly ISqlLiteRepositoryContextFactory _contextFactory;
        public UserService(ISqlLiteRepositoryContextFactory sqlLiteRepositoryContextFactory)
        {
            _contextFactory = sqlLiteRepositoryContextFactory;
        }

        public async Task AddUserAsync(UserView item)
        {
            using (var uow = new UnitOfWork(_contextFactory.Create()))
            {
                var userEntity = uow.UserRepository.GetEntityQuery().FirstOrDefault(x => x.Login == item.Login);
                if (userEntity != null)
                    throw new Exception("Такой пользователь уже существует");

                await uow.UserRepository.AddAsync(new User()
                {
                    Login = item.Login,
                    Password = item.Password,
                    Role = item.Role
                } ?? throw new InvalidOperationException());
            }
        }

        public async Task UpdateUserAsync(UserView item)
        {
            using (var uow = new UnitOfWork(_contextFactory.Create()))
            {
                await uow.UserRepository.UpdateAsync(new User()
                {
                    Id = item.Id ?? throw new Exception("Не получилось изменить пользователя"),
                    Login = item.Login,
                    Password = item.Password,
                    Role = item.Role
                });
            }
        }

        public async Task<IEnumerable<UserView>> RemoveRangeAsync(int[] ids)
        {
            IEnumerable<UserView> deletedUsers;
            using (var uow = new UnitOfWork(_contextFactory.Create()))
            {
                deletedUsers = (await uow.UserRepository
                        .RemoveRangeAsync(x => ids.Contains(x.Id)))
                    .Select(x => new UserView()
                    {
                        Id = x.Id,
                        Login = x.Login,
                        Password = x.Password,
                        Role = x.Role
                    });
            }

            return deletedUsers;
        }

        public IEnumerable<UserView> GetAllUsers()
        {
            IEnumerable<UserView> users;
            using (var uow = new UnitOfWork(_contextFactory.Create()))
            {
                users = uow.UserRepository.GetEntityQuery().Select(x => new UserView()
                {
                    Id = x.Id, 
                    Login = x.Login, 
                    Password = x.Password,
                    Role =x.Role
                }).ToList();
            }

            return users;
        }

        public UserView? GetAccountByLoginPassword(string login, string password)
        {
            UserView? user;
            using var uow = new UnitOfWork(_contextFactory.Create());

            var u = uow.UserRepository.GetEntityQuery().FirstOrDefault(x => x.Login == login && x.Password == password);
            if (u == null)
            {
                return null;
            }
            user = new UserView()
            {
                Id = u.Id,
                Login = u.Login, 
                Password = u.Password,
                Role = u.Role
            };
            return user;
        }

        public bool UserIsYetRegister(string login)
        {
            using var uow = new UnitOfWork(_contextFactory.Create());
            var user = uow.UserRepository.GetEntityQuery().FirstOrDefault(x => x.Login == login);
            if (user != null)
            {
                return true;
            }

            return false;
        }

        public ICollection<string> GetRolesCollection()
        {
            ICollection<string> roles;
            using (var uow = new UnitOfWork(_contextFactory.Create()))
            {
                roles = uow.UserRepository.GetEntityQuery().Select(x => x.Role).Distinct().ToList();
            }

            return roles;
        }
    }
}
