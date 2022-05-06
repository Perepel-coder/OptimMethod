using Models;
using Repository.interfaces;

namespace Repository.repositories;

public class UnitOfMeasRepository : DataBaseRepository<UnitOfMeas, RepositoryContext>, IUnitOfMeasRepository
{
    public UnitOfMeasRepository(RepositoryContext context) : base(context)
    {
    }

}

public class TasksRepository : DataBaseRepository<DescriptionTask, RepositoryContext>, ITasksRepository
{
    public TasksRepository(RepositoryContext context) : base(context)
    {
    }

}