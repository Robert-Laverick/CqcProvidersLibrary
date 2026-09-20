using CqcProvidersLibrary.API;
using CqcProvidersLibrary.Repository;
using System.Net.Http.Json;

namespace CqcProvidersLibrary
{
    public class CqcProvidersRepository
    {
        private static DateOnly MaxCacheAge => DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-1));
        private const int MaxPerPage = 100;
        private readonly ICqcProvidersEndpoint _endpoint;
        private readonly ICqcProvidersDatastore? _datastore;

        public CqcProvidersRepository(string subscriptionKey, string? connectionString = null)
        {
            _endpoint = new CqcProvidersEndpoint(subscriptionKey);
            if(!string.IsNullOrWhiteSpace(connectionString))
                _datastore = new CqcProvidersDatastore(connectionString);
        }

        public CqcProvidersRepository(ICqcProvidersEndpoint endpoint, ICqcProvidersDatastore datastore) { 
            _endpoint = endpoint;
            _datastore = datastore;
        }

        public async IAsyncEnumerable<ProvidersResponseLineDto> GetAllCqcProviders()
        {
            var request = new ProvidersRequest()
            {
                PerPage = MaxPerPage,
                Page = 1
            };

            // Do a GetReqest to the CQC API to get a list of providers
            var response = await _endpoint.GetCqcProviders(request);

            if (response is null)
                yield break;
            foreach (var provider in response.Providers)
            {
                yield return provider;
            }

            if(response.TotalPages > 1)
            {
                for (int page = 2; page <= response.TotalPages; page++)
                {
                    request.Page = page;
                    response = await _endpoint.GetCqcProviders(request);
                    if (response is null)
                        yield break;
                    foreach (var provider in response.Providers)
                    {
                        yield return provider;
                    }
                }
            }
        }

        public async Task<IEnumerable<ProvidersResponseLineDto>?> GetCqcProviders(ProvidersRequest request)
        {
            // Do a GetReqest to the CQC API to get a list of providers
            var response = await _endpoint.GetCqcProviders(request);

            if (response is null)
                return null;

            return response.Providers;
        }

        public async Task<CqcProvider?> GetCqcProviderById(string id)
        {
            if (_datastore is not null)
            {
                //try to get the provider from the datastore first
                var providerFromDatastore = await _datastore.GetProviderById(id);

                // if the provider is found in the datastore and the cached date is within the max cache age, return it
                if (providerFromDatastore is not null && providerFromDatastore.CachedDate > MaxCacheAge)
                    return providerFromDatastore;

            }
            // Do a GetRequest to the CQC API to get a provider by ID
            var response = await _endpoint.GetCqcProviderById(id);

            if (response is null)
                return null;

            var result = new CqcProvider(response);

            if (_datastore is not null)
            {
                // Insert the provider into the datastore
                await _datastore.InsertOrUpdateProvider(result);
            }

            return result;
        }
    }
}
