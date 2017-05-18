using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapStructure
{
    public class DependencyInfo
    {
        public Type Contract { get; set; }
        public Type Implementation { get; set; }
        public DependencyScope Scope { get; set; }
        public Func<IContainer, object> Factory { get; set; }
        public object Instance { get; set; }
    }
}
