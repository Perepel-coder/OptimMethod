using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<DescriptionTask> Tasks { get; set; } = null!;
        public DbSet<TaskParameterValue> Values { get; set; } = null!;
        public DbSet<UnitOfMeas> UnitsOfMeas { get; set; } = null!;
        public DbSet<Parameter> Parameters { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Method> MethodsOptimization { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(new User { Id = 1, Login = "admin", Password = "admin", Role = "admin" });
            modelBuilder.Entity<User>().HasData(new User { Id = 2, Login = "user", Password = "user", Role = "user" });

            // Единицы измерения
            var unitOfMeasMeter = new UnitOfMeas() { Id = 1, Name = "м" };
            var unitOfMeasKgMeter = new UnitOfMeas() { Id = 2, Name = "кг/м^3" };
            var unitOfMeasDgKgC = new UnitOfMeas() { Id = 3, Name = "Дж/(кг·°С)" };
            var unitOfMeasC = new UnitOfMeas() { Id = 4, Name = "°С" };
            var unitOfMeasMC = new UnitOfMeas() { Id = 5, Name = "м/с" };
            var unitOfMeasPaC = new UnitOfMeas() { Id = 6, Name = "Па·с^n" };
            var unitOfMeasOneOnC = new UnitOfMeas() { Id = 7, Name = "1/°С" };
            var unitOfMeasBtMC = new UnitOfMeas() { Id = 8, Name = "Вт/(м2·°С)" };
            var unitOfMeasNull = new UnitOfMeas() { Id = 9, Name = "-" };

            modelBuilder.Entity<UnitOfMeas>().HasData(unitOfMeasMeter, unitOfMeasKgMeter, unitOfMeasDgKgC, unitOfMeasC,
                unitOfMeasMC, unitOfMeasPaC, unitOfMeasOneOnC, unitOfMeasBtMC, unitOfMeasNull);

            // Ключи
            modelBuilder.Entity<TaskParameterValue>()
                .HasKey(t => new { t.DescriptionTaskId, t.ParameterId });

            modelBuilder.Entity<TaskParameterValue>()
                .HasOne(pt => pt.DescriptionTask)
                .WithMany(p => p.Values)
                .HasForeignKey(pt => pt.DescriptionTaskId);

            modelBuilder.Entity<TaskParameterValue>()
                .HasOne(pt => pt.Parameter)
                .WithMany(t => t.Values)
                .HasForeignKey(pt => pt.ParameterId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
