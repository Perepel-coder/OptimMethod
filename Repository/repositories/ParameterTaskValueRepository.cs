using Models;
using Repository.interfaces;

namespace Repository.repositories;

public class ParameterTaskValueRepository : DataBaseRepository<TaskParameterValue, RepositoryContext>, IParameterTaskValueRepository
{
    public ParameterTaskValueRepository(RepositoryContext context) : base(context)
    {
    }

}