using CqcProvidersLibrary.API;
using System;
using System.Collections.Generic;
using System.Text;

namespace CqcProvidersLibrary.Test
{
    internal class FakeEndpoint : ICqcProvidersEndpoint
    {
        public List<ProviderDto> Providers { get; set; } = new();

        public Task<ProviderDto?> GetCqcProviderById(string id, CancellationToken cancellationToken = default)
        {
            var provider = Providers.FirstOrDefault(p => p.ProviderId == id);
            return Task.FromResult(provider);
        }

        public Task<ProvidersResponseDto?> GetCqcProviders(ProvidersRequest request, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<ProvidersResponseDto?>(new ProvidersResponseDto()
            {
                Total = Providers.Count,
                Page = request.Page,
                PerPage = request.PerPage,
                TotalPages = (int)Math.Ceiling((double)Providers.Count / request.PerPage),
                FirstPageUri = "fake/first",
                LastPageUri = "fake/last",
                NextPageUri = "fake/next",
                PreviousPageUri = "fake/previous",
                Providers = Providers.Skip((request.Page - 1) * request.PerPage).Take(request.PerPage).Select(p => new ProvidersResponseLineDto() { ProviderId = p.ProviderId, ProviderName = p.Name }),
                Uri = "fake/uri"
            });
        }
    }
}
