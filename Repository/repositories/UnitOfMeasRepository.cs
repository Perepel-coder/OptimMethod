using Models;
using Repository.interfaces;

namespace Repository.repositories;

public class UnitOfMeasRepository : DataBaseRepository<UnitOfMeas, RepositoryContext>, IUnitOfMeasRepository
{
    public UnitOfMeasRepository(RepositoryContext context) : base(context)
    {
    }

}