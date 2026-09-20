using CqcProvidersLibrary.API;
using CqcProvidersLibrary.Repository;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;

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

        /// <summary>
        /// Get all CQC providers from the API. The API is paginated, itterating this will do multiple API reqests to ge all providers.
        /// </summary>
        /// <param name="request">Optional request parameters for filtering, pagination parameters are ignored as this request gets all providers</param>
        /// <param name="cancellationToken">Token to cancel the enumeration and any in-flight API requests</param>
        /// <returns></returns>
        public async IAsyncEnumerable<ProvidersResponseLineDto> GetAllCqcProviders(
            ProvidersRequest? request = null,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            if (request is null)
                request = new ProvidersRequest();

            request.PerPage = MaxPerPage;
            request.Page = 1;

            // Do a GetReqest to the CQC API to get a list of providers
            var response = await _endpoint.GetCqcProviders(request, cancellationToken);

            if (response is null)
                yield break;
            foreach (var provider in response.Providers)
            {
                cancellationToken.ThrowIfCancellationRequested();
                yield return provider;
            }

            if (response.TotalPages > 1)
            {
                for (int page = 2; page <= response.TotalPages; page++)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    request.Page = page;
                    response = await _endpoint.GetCqcProviders(request, cancellationToken);
                    if (response is null)
                        yield break;
                    foreach (var provider in response.Providers)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        yield return provider;
                    }
                }
            }
        }
        /// <summary>
        /// Get a single page of CQC providers from the API.
        /// </summary>
        /// <param name="request">Optional parameters for filtering and pagination</param>
        /// <returns></returns>

        public async Task<IEnumerable<ProvidersResponseLineDto>?> GetCqcProviders(ProvidersRequest? request = null, CancellationToken cancellationToken = default)
        {
            if (request is null)
                request = new ProvidersRequest();

            // Do a GetReqest to the CQC API to get a list of providers
            var response = await _endpoint.GetCqcProviders(request, cancellationToken);

            if (response is null)
                return null;

            return response.Providers;
        }

        /// <summary>
        /// Retrieve a CQC provider by its ID. 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="forceRefresh">If true, forces a refresh from the API even if a cached version is available.</param>
        /// <returns></returns>
        public async Task<CqcProvider?> GetCqcProviderById(string id, bool forceRefresh = false, CancellationToken cancellationToken = default)
        {
            if (_datastore is not null && !forceRefresh)
            {
                //try to get the provider from the datastore first
                var providerFromDatastore = await _datastore.GetProviderById(id);

                // if the provider is found in the datastore and the cached date is within the max cache age, return it
                if (providerFromDatastore is not null && providerFromDatastore.CachedDate > MaxCacheAge)
                    return providerFromDatastore;

            }
            // Do a GetRequest to the CQC API to get a provider by ID
            var response = await _endpoint.GetCqcProviderById(id, cancellationToken);

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
