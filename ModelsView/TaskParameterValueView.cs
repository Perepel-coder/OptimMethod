using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsView
{
    public class TaskParameterValueView
    {
        public int ParameterId { get; set; }
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public string Description { get; set; }
        public string Notation { get; set; }
        public string UnitOfMeasName { get; set; }
        public double Value { get; set; }
    }
}
