using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.factories;
using Services.Interfaces;

namespace Services
{
    public class TasksService: ITasksService
    {
        private readonly ISqlLiteRepositoryContextFactory _repositoryContext;
        public TasksService(ISqlLiteRepositoryContextFactory repository)
        {
            _repositoryContext = repository;
        }
    }
}
