using CqcProvidersLibrary.API;
using System;
using System.Collections.Generic;
using System.Text;

namespace CqcProvidersLibrary.Test
{
    internal class FakeEndpoint : ICqcProvidersEndpoint
    {
        public Dictionary<string, ProviderDto> Providers { get; set; } = new Dictionary<string, ProviderDto>();

        public Task<ProviderDto?> GetCqcProviderById(string id)
        {
            Providers.TryGetValue(id, out var provider);
            return Task.FromResult(provider);
        }

        public Task<IEnumerable<ProvidersResponseLineDto>> GetCqcProviders()
        {
            return Task.FromResult(Providers.Values.Select(p => new ProvidersResponseLineDto() { ProviderId = p.ProviderId, ProviderName = p.Name }));
        }
    }
}
