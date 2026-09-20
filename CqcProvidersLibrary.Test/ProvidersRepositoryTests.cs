using CqcProvidersLibrary.Repository;
using Microsoft.Extensions.Configuration;

namespace CqcProvidersLibrary.Test
{
    public class ProvidersRepositoryTests
    {
        private readonly IConfiguration _configuration;
        private string _subscriptionKey => _configuration["CqcSubscriptionKey"] 
            ?? throw new InvalidOperationException("CqcSubscriptionKey is not configured in user secrets.");
        public ProvidersRepositoryTests()
        {
            _configuration = new ConfigurationBuilder()
                .AddUserSecrets<ProvidersRepositoryTests>()
                .Build();
        }

        [Fact]
        public async Task BasicCqcProviderTests()
        {
            var repository = new CqcProvidersRepository(_subscriptionKey);
            var providers = await repository.GetCqcProviders();
            Assert.NotNull(providers);

            var firstProvider = providers.FirstOrDefault();
            Assert.NotNull(firstProvider);
            Assert.False(string.IsNullOrEmpty(firstProvider.ProviderId));

            var providerById = await repository.GetCqcProviderById(firstProvider.ProviderId);
            Assert.NotNull(providerById);

        }
    }
}
