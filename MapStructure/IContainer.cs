using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapStructure
{
    public interface IContainer
    {
        void Register<TContract, TImplementation>();
        void Register<TContract, TImplementation>(DependencyScope scope);
        void Register<TContract>(Func<IContainer, object> factory);
        TContract Resolve<TContract>();
        object Resolve(Type contractType);
    }
}
