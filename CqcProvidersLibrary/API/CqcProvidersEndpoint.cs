using CqcProvidersLibrary.Repository;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using System.Web;

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

        public async Task<ProvidersResponseDto?> GetCqcProviders(ProvidersRequest request, CancellationToken cancellationToken = default)
        {
            // Do a GetReqest to the CQC API to get a list of providers

            var builder = new UriBuilder(CqcBaseUrl + CqcProvidersApiUrl);

            var query = HttpUtility.ParseQueryString(builder.Query);
            foreach (var param in request.GetParameters())
            {
                query[param.Key] = param.Value;
            }

            builder.Query = query.ToString();
            var url = builder.ToString();

            var response = await _httpClient.GetFromJsonAsync<ProvidersResponseDto>(url, cancellationToken);

            return response;
        }

        public async Task<ProviderDto?> GetCqcProviderById(string id, CancellationToken cancellationToken = default)
        {
            // Do a GetRequest to the CQC API to get a provider by ID
            try
            {
                var providerDto = await _httpClient.GetFromJsonAsync<ProviderDto>(string.Format(CqcProviderByIdApiUrlTemplate, id), cancellationToken);

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
