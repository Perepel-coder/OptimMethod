using Repository.factories;
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
    }
}
