using Repository.interfaces;
using Repository.repositories;

namespace Repository.UOW
{
    public class UnitOfWork : IDisposable
    {
        protected readonly RepositoryContext _repositoryContext;
        public readonly IUserRepository UserRepository;
        public readonly IParameterRepository ParameterRepository;
        public readonly IUnitOfMeasRepository UnitOfMeasRepository;
        public readonly IParameterTaskValueRepository ParameterTaskValueRepository;
        public readonly IMethodOptimizationRepository MethodOptimizationRepository;

        public UnitOfWork(RepositoryContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            _repositoryContext = context;
            UserRepository = new UserRepository(_repositoryContext);
            ParameterRepository = new ParameterRepository(_repositoryContext);
            UnitOfMeasRepository = new UnitOfMeasRepository(_repositoryContext);
            ParameterTaskValueRepository = new ParameterTaskValueRepository(_repositoryContext);
            MethodOptimizationRepository = new MathodOptimizationRepository(_repositoryContext);
        }

        private bool disposed = false;
        public void Save()
        {
            _repositoryContext.SaveChanges();
        }
        public async Task SaveAsync()
        {
            await _repositoryContext.SaveChangesAsync();
        }
        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _repositoryContext.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
