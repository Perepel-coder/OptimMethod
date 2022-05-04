using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelsView;

namespace Services.Interfaces
{
    public interface IMethodService
    {
        Task<ICollection<OptimizationMethodView>> GetAllOptimizationMethodsAsync();
        ICollection<OptimizationMethodView> GetAllOptimizationMethods();
        Task DeleteOptimizationMethodAsync(int idMethod);
        Task AddOptimizationMethodAsync(string name, bool isRealised);
        Task EditOptimizationMethod(int id, string description, bool isRealised);
    }
}
