using CommonServiceLocator;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection.Prism
{
    public class MEDIRegionNavigationContentLoader : RegionNavigationContentLoader
    {
        private readonly IServiceCollection _serviceCollection;

        public MEDIRegionNavigationContentLoader(IServiceLocator serviceLocator, IServiceCollection serviceCollection) : base(serviceLocator)
        {
            _serviceCollection = NamedNullException.Assert(serviceCollection, nameof(serviceCollection));
        }

        protected unsafe override IEnumerable<object> GetCandidatesFromRegion(
            IRegion region,
            string candidateNavigationContract)
        {
            StringNullOrEmptyException.Assert(candidateNavigationContract, nameof(candidateNavigationContract));

            var contractCandidates = base.GetCandidatesFromRegion(region, candidateNavigationContract);
            if (!contractCandidates.Any())
            {
                var matchingRegistration = _serviceCollection
                    .Where(r => candidateNavigationContract.Equals(r.ServiceType.Name, StringComparison.Ordinal))
                    .FirstOrDefault();
                if (matchingRegistration == null) return new object[0];

                string typeCandidateName = matchingRegistration.ImplementationType.FullName;
                contractCandidates = base.GetCandidatesFromRegion(region, typeCandidateName);
            }

            return contractCandidates;
        }
    }
}
