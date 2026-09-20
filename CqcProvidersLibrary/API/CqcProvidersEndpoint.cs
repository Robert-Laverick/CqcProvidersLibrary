using CqcProvidersLibrary.Repository;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace CqcProvidersLibrary.API
{
    public class CqcProvidersEndpoint: ICqcProvidersEndpoint
    {
        const string CqcBaseUrl = "https://api.service.cqc.org.uk/public/v1/"; // Base URL for CQC API
        const string CqcProvidersApiUrl = "providers"; // Api endpoint for getting a list of providers
        const string CqcProviderByIdApiUrlTemplate = "providers/{0}"; // Api endpoint for getting provider by ID
        HttpClient _httpClient;

        public CqcProvidersEndpoint(string subscriptionKey)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(CqcBaseUrl)
            };
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
        }

        public async Task<IEnumerable<ProvidersResponseLineDto>> GetCqcProviders()
        {
            // Do a GetReqest to the CQC API to get a list of providers
            var response = await _httpClient.GetFromJsonAsync<ProvidersResponseDto>(CqcProvidersApiUrl);


            return response.Providers;
        }

        public async Task<ProviderDto?> GetCqcProviderById(string id)
        {
            // Do a GetRequest to the CQC API to get a provider by ID
            try
            {
                var providerDto = await _httpClient.GetFromJsonAsync<ProviderDto>(string.Format(CqcProviderByIdApiUrlTemplate, id));

                return providerDto;
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    // Handle the case when the provider is not found (404) - return null when the provider is not found;
                    return null;
                }
                throw; // Re-throw the exception for other HTTP errors
            }
        }
    }
}
