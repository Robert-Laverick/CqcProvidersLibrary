using CqcProvidersLibrary.API;
using CqcProvidersLibrary.Repository;
using System.Net.Http.Json;

namespace CqcProvidersLibrary
{
    public class CqcProvidersRepository
    {
        private static DateOnly MaxCacheAge => DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-1));
        private readonly ICqcProvidersEndpoint _endpoint;
        private readonly ICqcProvidersDatastore _datastore;

        public CqcProvidersRepository(ICqcProvidersEndpoint endpoint, ICqcProvidersDatastore datastore) { 
            _endpoint = endpoint;
            _datastore = datastore;
        }

        public async Task<IEnumerable<ProvidersResponseLineDto>> GetCqcProviders()
        {
            // Do a GetReqest to the CQC API to get a list of providers
            var response = await _endpoint.GetCqcProviders();

            return response;
        }

        public async Task<CqcProvider?> GetCqcProviderById(string id)
        {
            //try to get the provider from the datastore first
            var providerFromDatastore = await _datastore.GetProviderById(id);

            // if the provider is found in the datastore and the cached date is within the max cache age, return it
            if (providerFromDatastore is not null && providerFromDatastore.CachedDate > MaxCacheAge)
                return providerFromDatastore;

            // Do a GetRequest to the CQC API to get a provider by ID
            var response = await _endpoint.GetCqcProviderById(id);

            if (response is null)
                return null;

            var result = new CqcProvider(response);

            // Insert the provider into the datastore
            await _datastore.InsertOrUpdateProvider(result);

            return result;
        }
    }
}
