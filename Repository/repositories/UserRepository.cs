using System.ComponentModel.DataAnnotations;
using System.Linq;
using Models;
using Repository.interfaces;

namespace Repository.repositories;

public class UserRepository : DataBaseRepository<User, RepositoryContext>, IUserRepository
{
    public UserRepository(RepositoryContext context) : base(context)
    {
    }

    public override async Task AddAsync(User item)
    {
        var users = GetEntityQuery().Where(x => x.Login == item.Login);
        if (users.Any())
        {
            throw new ValidationException("Такой логин уже есть");
        }

        await base.AddAsync(item);
    }
}