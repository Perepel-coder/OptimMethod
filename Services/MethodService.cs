using Microsoft.EntityFrameworkCore;
using Models;
using ModelsView;
using Repository.factories;
using Repository.UOW;
using Services.Interfaces;

namespace Services
{
    public class MethodService: IMethodService
    {
        private readonly ISqlLiteRepositoryContextFactory _repositoryContext;
        public MethodService(ISqlLiteRepositoryContextFactory repository)
        {
            _repositoryContext = repository;
        }

        public async Task<ICollection<OptimizationMethodView>> GetAllOptimizationMethodsAsync()
        {
            using var uow = new UnitOfWork(_repositoryContext.Create());
            ICollection<OptimizationMethodView> optimizationMethods = await uow.MethodOptimizationRepository
                .GetEntityQuery()
                .Select(x => new OptimizationMethodView()
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsRealized = x.IsRealized
                }).ToListAsync();
            return optimizationMethods;
        }

        public ICollection<OptimizationMethodView> GetAllOptimizationMethods()
        {
            using var uow = new UnitOfWork(_repositoryContext.Create());
            ICollection<OptimizationMethodView> optimizationMethods = uow.MethodOptimizationRepository
                .GetEntityQuery()
                .Select(x => new OptimizationMethodView()
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsRealized = x.IsRealized
                }).ToList();
            return optimizationMethods;
        }

        public async Task DeleteOptimizationMethodAsync(int idMethod)
        {
            using var uow = new UnitOfWork(_repositoryContext.Create());
            await uow.MethodOptimizationRepository.RemoveRangeAsync(x => x.Id == idMethod);
        }

        public async Task AddOptimizationMethodAsync(string name, bool isRealised)
        {
            using var uow = new UnitOfWork(_repositoryContext.Create());
            await uow.MethodOptimizationRepository.AddAsync(new Method()
            {
                IsRealized = isRealised,
                Name = name
            });
        }

        public async Task EditOptimizationMethod(int id, string description, bool isRealised)
        {
            using var uow = new UnitOfWork(_repositoryContext.Create());
            await uow.MethodOptimizationRepository.UpdateAsync(new Method()
            {
                Id = id,
                IsRealized = isRealised,
                Name = description
            });
        }
    }
}
