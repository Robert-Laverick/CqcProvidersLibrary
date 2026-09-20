using CqcProvidersLibrary.API;
using System.Net.Http.Json;

namespace CqcProvidersLibrary.Repository
{
    public class CqcProvidersRepository
    {
        const string CqcBaseUrl = "https://api.service.cqc.org.uk/public/v1/"; // Base URL for CQC API
        const string CqcProvidersApiUrl = "providers"; // Api endpoint for getting a list of providers
        const string CqcProviderByIdApiUrlTemplate = "providers/{0}"; // Api endpoint for getting provider by ID
        HttpClient _httpClient;

        public CqcProvidersRepository(string subscriptionKey) { 
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(CqcBaseUrl)
            };
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
        }

        public async Task<IEnumerable<CqcProvidersResponseLineDto>> GetCqcProviders()
        {
            // Do a GetReqest to the CQC API to get a list of providers
            var response = await _httpClient.GetFromJsonAsync<ProvidersResponseDto>(CqcProvidersApiUrl);


            return response.Providers;
        }

        public async Task<CqcProvider?> GetCqcProviderById(string id)
        {
            // Do a GetRequest to the CQC API to get a provider by ID
            var provider = await _httpClient.GetFromJsonAsync<CqcProvider>(string.Format(CqcProviderByIdApiUrlTemplate, id));

            return provider;
        }

    }
}
