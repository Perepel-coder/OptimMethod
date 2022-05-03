using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelsView;

namespace Services.Interfaces
{
    public interface IUnitOfMeasService
    {
        Task<ICollection<string>> GetAllNamesUnitOfMeasAsync();
        ICollection<string> GetAllNamesUnitOfMeas();
        Task<ICollection<UnitOfMeasView>> GetAllUnitOfMeasAsync();
        ICollection<UnitOfMeasView> GetAllUnitsOfMeas();
        Task AddUnitOfMeasAsync(string name);
        Task RemoveUnitOfMeasAsync(int id);
    }
}
