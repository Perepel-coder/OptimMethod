using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;
using ModelsView;
using Repository.factories;
using Repository.interfaces;
using Repository.UOW;
using Services.Interfaces;

namespace Services
{
    public class UnitOfMeasService: IUnitOfMeasService
    {
        private readonly ISqlLiteRepositoryContextFactory _repositoryContext;
        public UnitOfMeasService(ISqlLiteRepositoryContextFactory repository)
        {
            _repositoryContext = repository;
        }

        public async Task<ICollection<string>> GetAllNamesUnitOfMeasAsync()
        {
            using var uow = new UnitOfWork(_repositoryContext.Create());
            ICollection<string> names = await uow.UnitOfMeasRepository.GetEntityQuery().Select(x => x.Name).ToListAsync();
            return names;
        }

        public ICollection<string> GetAllNamesUnitOfMeas()
        {
            using var uow = new UnitOfWork(_repositoryContext.Create());
            ICollection<string> names = uow.UnitOfMeasRepository.GetEntityQuery().Select(x => x.Name).ToList();
            return names;
        }

        public async Task<ICollection<UnitOfMeasView>> GetAllUnitOfMeasAsync()
        {
            using var uow = new UnitOfWork(_repositoryContext.Create());
            
            return await uow.UnitOfMeasRepository.GetEntityQuery().Select(x => new UnitOfMeasView()
                {
                    Id = x.Id,
                    Name = x.Name,
                })
                .ToListAsync();
        }

        public ICollection<UnitOfMeasView> GetAllUnitsOfMeas()
        {
            using var uow = new UnitOfWork(_repositoryContext.Create());

            return uow.UnitOfMeasRepository.GetEntityQuery().Select(x => new UnitOfMeasView()
                {
                    Id = x.Id,
                    Name = x.Name,
                })
                .ToList();
        }

        public async Task AddUnitOfMeasAsync(string name)
        {
            using var uow = new UnitOfWork(_repositoryContext.Create());
            await uow.UnitOfMeasRepository.AddAsync(new UnitOfMeas(){Name = name});
        }

        public async Task RemoveUnitOfMeasAsync(int id)
        {
            using var uow = new UnitOfWork(_repositoryContext.Create());
            await uow.UnitOfMeasRepository.RemoveRangeAsync(x => x.Id == id);
        }
    }
}
