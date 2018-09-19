using CommonServiceLocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection.Prism
{
    public class MEDIServiceLocatorAdapter : ServiceLocatorImplBase
    {
        private readonly IServiceProvider _serviceProvider;
        public MEDIServiceLocatorAdapter(IServiceProvider serviceProvider)
        {
            _serviceProvider = NamedNullException.Assert(serviceProvider, nameof(serviceProvider));
        }

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            return _serviceProvider.GetServices(serviceType);
        }

        protected override object DoGetInstance(Type serviceType, string key)
        {
            throw new NotImplementedException();
        }
    }
}
