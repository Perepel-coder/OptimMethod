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
    public class ParametersService: IParameterService
    {
        private readonly ISqlLiteRepositoryContextFactory _repositoryContext;

        public ParametersService(ISqlLiteRepositoryContextFactory repository)
        {
            _repositoryContext = repository;
        }

        public async Task<ICollection<ParameterView>> GetAllParametersAsync()
        {
            ICollection<ParameterView> parameters;
            using var uow = new UnitOfWork(_repositoryContext.Create());
            var param = uow.ParameterRepository.GetEntityQuery().Select(x => new ParameterView()
            {
                Id = x.Id,
                Description = x.Description,
                Notation = x.Notation,
                UnitOfMeasName = x.UnitOfMeas.Name
            });
            parameters = await param.ToListAsync();

            return parameters;
        }

        public ICollection<ParameterView> GetAllParameters()
        {
            ICollection<ParameterView> parameters;
            using var uow = new UnitOfWork(_repositoryContext.Create());
            var param = uow.ParameterRepository.GetEntityQuery().Select(x => new ParameterView()
            {
                Id = x.Id,
                Description = x.Description,
                Notation = x.Notation,
                UnitOfMeasName = x.UnitOfMeas.Name
            });
            parameters = param.ToList();

            return parameters;
        }

        public async Task AddParameterAsync(ParameterView parameter)
        {
            using var uow = new UnitOfWork(_repositoryContext.Create());
            UnitOfMeas? unitOfMeas = await uow.UnitOfMeasRepository.GetEntityQuery()
                .FirstOrDefaultAsync(x => x.Name == parameter.UnitOfMeasName);
            if (unitOfMeas == null)
            {
                await uow.ParameterRepository.AddAsync( new Parameter()
                {
                    Description = parameter.Description,
                    UnitOfMeas = new UnitOfMeas()
                    {
                        Name = parameter.UnitOfMeasName
                    }
                });
            }
            else
            {
                await uow.ParameterRepository.AddAsync(new Parameter()
                {
                    Description = parameter.Description,
                    Notation = parameter.Notation,
                    UnitOfMeasId = unitOfMeas.Id
                });
            }
        }

        public async Task DeleteParameterAsync(int id)
        {
            using var uow = new UnitOfWork(_repositoryContext.Create());
            await uow.ParameterRepository.RemoveRangeAsync(x => x.Id == id);
        }

        public async Task EditParameterAsync(ParameterView parameter)
        {
            if(parameter.Id == null)
                return;
            var uow = new UnitOfWork(_repositoryContext.Create());
            var unitOfMeas = await uow.UnitOfMeasRepository.GetEntityQuery()
                .FirstOrDefaultAsync(x => x.Name == parameter.UnitOfMeasName);
            if (unitOfMeas == null)
            {
                throw new Exception("Единица измерения не найдена");
            }
            await uow.ParameterRepository.UpdateAsync(new Parameter()
            {
                Id = parameter.Id ?? throw new Exception("Ошибка при изменении параметра: пустой id "),
                Description = parameter.Description,
                Notation = parameter.Notation,
                UnitOfMeasId = unitOfMeas.Id
            });
        }
    }
}
