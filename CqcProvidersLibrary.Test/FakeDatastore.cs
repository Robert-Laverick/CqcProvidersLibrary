using CqcProvidersLibrary.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace CqcProvidersLibrary.Test
{
    internal class FakeDatastore : ICqcProvidersDatastore
    {
        public Dictionary<string, CqcProvider> _providers = new Dictionary<string, CqcProvider>();

        public Task<CqcProvider?> GetProviderById(string id)
        {
            _providers.TryGetValue(id, out var provider);
            return Task.FromResult(provider);
        }

        public Task InsertOrUpdateProvider(CqcProvider provider)
        {
            _providers[provider.Id] = provider;
            return Task.CompletedTask;
        }
    }
}
