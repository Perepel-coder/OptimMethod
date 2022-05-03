using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelsView;

namespace Services.Interfaces
{
    public interface IParameterService
    {
        Task<ICollection<ParameterView>> GetAllParametersAsync();
        ICollection<ParameterView> GetAllParameters();
        Task AddParameterAsync(ParameterView parameter);
        Task DeleteParameterAsync(int id);
        Task EditParameterAsync(ParameterView parameter);
    }
}
