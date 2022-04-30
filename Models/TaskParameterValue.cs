using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class TaskParameterValue
    {
        public int ParameterId { get; set; }
        public Parameter Parameter { get; set; } = null!;
        public int TaskId { get; set; }
        public DescriptionTask DescriptionTask { get; set; } = null!;
        public double Value { get; set; }
    }
}
