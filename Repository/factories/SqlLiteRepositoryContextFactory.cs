using Microsoft.EntityFrameworkCore;

namespace Repository.factories
{
    public class SqlLiteRepositoryContextFactory : ISqlLiteRepositoryContextFactory
    {
        private string _connectionString;

        public SqlLiteRepositoryContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public RepositoryContext Create()
        {
            //string connectionString = "Data Source = rpkDB.db";

            var optionsBuilder = new DbContextOptionsBuilder<RepositoryContext>();
            optionsBuilder.UseSqlite(_connectionString);
            //optionsBuilder.LogTo(Console.WriteLine);
            //optionsBuilder.EnableSensitiveDataLogging();

            return new RepositoryContext(optionsBuilder.Options);
        }
    }
}
