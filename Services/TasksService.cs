using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;
using ModelsView;
using Repository.factories;
using Repository.UOW;
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

        public async Task<ICollection<TaskView>> GetAllTaskAsync()
        {
            using var uow = new UnitOfWork(_repositoryContext.Create());
            ICollection<TaskView> tasks = await uow.TasksRepository.GetEntityQuery()
                .Select(x => new TaskView()
                {
                    Description = x.Description,
                    IdTask = x.Id,
                    Name = x.Name
                }).ToListAsync();
            return tasks;
        }

        public ICollection<TaskView> GetAllTask()
        {
            using var uow = new UnitOfWork(_repositoryContext.Create());
            ICollection<TaskView> tasks = uow.TasksRepository.GetEntityQuery()
                .Select(x => new TaskView()
                {
                    Description = x.Description,
                    IdTask = x.Id,
                    Name = x.Name,
                }).ToList();
            return tasks;
        }

        public async Task AddTaskAsync(string name, string desc)
        {
            using var uow = new UnitOfWork(_repositoryContext.Create());
            await uow.TasksRepository.AddAsync(new DescriptionTask()
            {
                Description = desc,
                Name = name
            });
        }

        public async Task DeleteTaskAsync(int idTask)
        {
            using var uow = new UnitOfWork(_repositoryContext.Create());
            await uow.TasksRepository.RemoveRangeAsync(x => x.Id == idTask);
        }

        public async Task EditTaskAsync(int idTask, string name, string desc)
        {
            using var uow = new UnitOfWork(_repositoryContext.Create());
            await uow.TasksRepository.UpdateAsync(new DescriptionTask()
            {
                Id = idTask,
                Name = name,
                Description = desc
            });
        }

        public ICollection<TaskParameterValueView> GetAllParametersValues()
        {
            using var uow = new UnitOfWork(_repositoryContext.Create());
            ICollection<TaskParameterValueView> parameters = uow.ParameterTaskValueRepository.GetEntityQuery()
                .Select(x => new TaskParameterValueView()
                {
                    TaskName = x.DescriptionTask.Name,
                    Description = x.Parameter.Description,
                    Notation = x.Parameter.Notation,
                    ParameterId = x.ParameterId,
                    TaskId = x.DescriptionTaskId,
                    UnitOfMeasName = x.Parameter.UnitOfMeas.Name,
                    Value = x.Value
                }).ToList();
            return parameters;
        }

        public async Task<ICollection<TaskParameterValueView>> GetAllParametersAsync()
        {
            using var uow = new UnitOfWork(_repositoryContext.Create());
            ICollection<TaskParameterValueView> parameters = await uow.ParameterTaskValueRepository.GetEntityQuery()
                .Select(x => new TaskParameterValueView()
                {
                    TaskName = x.DescriptionTask.Name,
                    Description = x.Parameter.Description,
                    Notation = x.Parameter.Notation,
                    ParameterId = x.ParameterId,
                    TaskId = x.DescriptionTaskId,
                    UnitOfMeasName = x.Parameter.UnitOfMeas.Name,
                    Value = x.Value
                }).ToListAsync();
            return parameters;
        }

        public async Task<ICollection<TaskParameterValueView>> GetParametersByTaskIdAsync(int taskId)
        {
            using var uow = new UnitOfWork(_repositoryContext.Create());
            ICollection<TaskParameterValueView> parameters = await uow.ParameterTaskValueRepository.GetEntityQuery()
                .Where(z => z.DescriptionTaskId == taskId)
                .Select(x => new TaskParameterValueView()
                {
                    TaskName = x.DescriptionTask.Name,
                    Description = x.Parameter.Description,
                    Notation = x.Parameter.Notation,
                    ParameterId = x.ParameterId,
                    TaskId = x.DescriptionTaskId,
                    UnitOfMeasName = x.Parameter.UnitOfMeas.Name,
                    Value = x.Value
                }).ToListAsync();
            return parameters;
        }
        public ICollection<TaskParameterValueView> GetParametersByTaskId(int taskId)
        {
            using var uow = new UnitOfWork(_repositoryContext.Create());
            ICollection<TaskParameterValueView> parameters = uow.ParameterTaskValueRepository.GetEntityQuery()
                .Where(z => z.DescriptionTaskId == taskId)
                .Select(x => new TaskParameterValueView()
                {
                    TaskName = x.DescriptionTask.Name,
                    Description = x.Parameter.Description,
                    Notation = x.Parameter.Notation,
                    ParameterId = x.ParameterId,
                    TaskId = x.DescriptionTaskId,
                    UnitOfMeasName = x.Parameter.UnitOfMeas.Name,
                    Value = x.Value
                }).ToList();
            return parameters;
        }
        public async Task AddParameterTaskAsync(int parameterId, int taskId, double value)
        {
            using var uow = new UnitOfWork(_repositoryContext.Create());
            await uow.ParameterTaskValueRepository.AddAsync(new TaskParameterValue()
            {
                ParameterId = parameterId,
                DescriptionTaskId = taskId,
                Value = value
            });
        }

        public async Task DeleteParameterTaskAsync(int parameterId, int taskId)
        {
            using var uow = new UnitOfWork(_repositoryContext.Create());
            await uow.ParameterTaskValueRepository.RemoveRangeAsync(x => x.DescriptionTaskId == taskId && x.ParameterId == parameterId);
        }

        public async Task EditParameterTaskAsync(int parameterId, int taskId, double value)
        {
            using var uow = new UnitOfWork(_repositoryContext.Create());
            await uow.ParameterTaskValueRepository.UpdateAsync(new TaskParameterValue()
            {
                DescriptionTaskId = taskId,
                ParameterId = parameterId,
                Value = value
            });
        }
    }
}
