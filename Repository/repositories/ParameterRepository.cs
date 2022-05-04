using Models;
using Repository.interfaces;

namespace Repository.repositories;

public class ParameterRepository : DataBaseRepository<Parameter, RepositoryContext>, IParameterRepository
{
    public ParameterRepository(RepositoryContext context) : base(context)
    {
    }

}


public class MathodOptimizationRepository : DataBaseRepository<Method, RepositoryContext>, IMethodOptimizationRepository
{
    public MathodOptimizationRepository(RepositoryContext context) : base(context)
    {
    }

}