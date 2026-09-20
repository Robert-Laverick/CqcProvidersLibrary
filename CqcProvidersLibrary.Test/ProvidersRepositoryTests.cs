using CqcProvidersLibrary.API;
using CqcProvidersLibrary.Repository;
using Microsoft.Extensions.Configuration;

namespace CqcProvidersLibrary.Test
{
    public class ProvidersRepositoryTests
    {
        private readonly IConfiguration _configuration;
        private string _subscriptionKey => _configuration["CqcSubscriptionKey"]
            ?? throw new InvalidOperationException("CqcSubscriptionKey is not configured in user secrets.");
        public static bool _subscriptionKeyMissing
        {
            get
            {
                var configuration = new ConfigurationBuilder()
                    .AddUserSecrets<ProvidersRepositoryTests>()
                    .Build();
                var subscriptionKey = configuration["CqcSubscriptionKey"];
                return string.IsNullOrWhiteSpace(subscriptionKey);
            }
        }
        private string? _connectionString => _configuration["ConnectionString"];
        public static bool _databaseConnectionStringMissing
        {
            get
            {
                var configuration = new ConfigurationBuilder()
                    .AddUserSecrets<ProvidersRepositoryTests>()
                    .Build();
                var subscriptionKey = configuration["ConnectionString"];
                return string.IsNullOrWhiteSpace(subscriptionKey);
            }
        }

        public static bool _skipTests => _subscriptionKeyMissing || _databaseConnectionStringMissing;

        public ProvidersRepositoryTests()
        {
            _configuration = new ConfigurationBuilder()
                .AddUserSecrets<ProvidersRepositoryTests>()
                .Build();
        }

        [Fact(Skip = "No API Subscription Key Set", SkipWhen = nameof(_subscriptionKeyMissing))]
        public async Task BasicCqcEndpointTests()
        {
            var repository = new CqcProvidersEndpoint(_subscriptionKey);
            var parameters = new ProvidersRequest
            {
                PerPage = 10,
                Page = 1
            };
            var result = await repository.GetCqcProviders(parameters, cancellationToken: TestContext.Current.CancellationToken);
            Assert.NotNull(result);
            var providers = result?.Providers;
            Assert.NotNull(providers);

            var firstProvider = providers.FirstOrDefault();
            Assert.NotNull(firstProvider);
            Assert.False(string.IsNullOrEmpty(firstProvider.ProviderId));

            var providerById = await repository.GetCqcProviderById(firstProvider.ProviderId, cancellationToken: TestContext.Current.CancellationToken);
            Assert.NotNull(providerById);

            var lastProvider = providers.LastOrDefault();
            Assert.NotNull(lastProvider);
            Assert.False(string.IsNullOrEmpty(lastProvider.ProviderId));

            var lastProviderById = await repository.GetCqcProviderById(lastProvider.ProviderId, cancellationToken: TestContext.Current.CancellationToken);
            Assert.NotNull(lastProviderById);
        }

        [Fact(Skip = "No API Subscription Key Set or Database Connection string missing", SkipWhen = nameof(_skipTests))]
        public async Task EndToEndTest()
        {
            var repository = new CqcProvidersRepository(_subscriptionKey, _connectionString);
            var providers = await repository.GetAllCqcProviders(cancellationToken: TestContext.Current.CancellationToken).Take(20).ToListAsync(cancellationToken: TestContext.Current.CancellationToken);
            Assert.NotNull(providers);
            var firstProvider = providers.FirstOrDefault();
            Assert.NotNull(firstProvider);
            Assert.False(string.IsNullOrEmpty(firstProvider.ProviderId));
            var providerById = await repository.GetCqcProviderById(firstProvider.ProviderId, cancellationToken: TestContext.Current.CancellationToken);
            Assert.NotNull(providerById);
        }

        [Fact]
        public async Task EndToEndTestWithFakeEndpointAndDatastore()
        {
            var fakeEndpoint = new FakeEndpoint();
            var fakeDatastore = new FakeDatastore();
            var repository = new CqcProvidersRepository(fakeEndpoint, fakeDatastore);

            // Add a provider to the fake endpoint
            var providerDto = new ProviderDto
            {
                ProviderId = "123",
                Name = "Test Provider"
            };
            fakeEndpoint.Providers.Add(providerDto);

            // Test GetCqcProviders
            var providers = await repository.GetCqcProviders(new ProvidersRequest(), cancellationToken: TestContext.Current.CancellationToken);
            Assert.NotNull(providers);
            Assert.Single(providers);
            Assert.Equal(providerDto.ProviderId, providers.First().ProviderId);

            // Test GetCqcProviderById
            var providerById = await repository.GetCqcProviderById(providerDto.ProviderId, cancellationToken: TestContext.Current.CancellationToken);
            Assert.NotNull(providerById);
            Assert.Equal(providerDto.ProviderId, providerById.Id);

            // Test caching in the fake datastore
            var cachedProvider = await fakeDatastore.GetProviderById(providerDto.ProviderId);
            Assert.NotNull(cachedProvider);
            Assert.Equal(providerDto.ProviderId, cachedProvider.Id);
        }

        [Fact]
        public async Task TestCacheRetrieval()
        {
            var fakeEndpoint = new FakeEndpoint();
            var fakeDatastore = new FakeDatastore();
            var repository = new CqcProvidersRepository(fakeEndpoint, fakeDatastore);

            // Add a provider to the fake endpoint
            var providerDto = new ProviderDto
            {
                ProviderId = "123",
                Name = "Test Provider"
            };
            
            var cachedProvider = new CqcProvider(providerDto)
            {
                CachedDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-10)) // Set cached date to 10 days ago
            };
            fakeDatastore._providers.Add(cachedProvider.Id, cachedProvider);

            // Test GetCqcProviderById
            var providerById = await repository.GetCqcProviderById(providerDto.ProviderId, cancellationToken: TestContext.Current.CancellationToken);
            Assert.NotNull(providerById);
            Assert.Equal(providerDto.ProviderId, providerById.Id);
            Assert.Equal(providerDto.Name, providerById.Name);

            // Check fake datastore was not updated
            var cachedProviderAfter = await fakeDatastore.GetProviderById(providerDto.ProviderId);
            Assert.NotNull(cachedProviderAfter);
            Assert.Equal(providerDto.ProviderId, cachedProviderAfter.Id);
            Assert.Equal(providerDto.Name, cachedProviderAfter.Name);
            Assert.Equal(cachedProvider.CachedDate, cachedProviderAfter.CachedDate); // Cached date should remain the same
        }

        [Fact]
        public async Task TestCacheExpiry()
        {
            var fakeEndpoint = new FakeEndpoint();
            var fakeDatastore = new FakeDatastore();
            var repository = new CqcProvidersRepository(fakeEndpoint, fakeDatastore);

            // Add a provider to the fake endpoint
            var providerDto = new ProviderDto
            {
                ProviderId = "123",
                Name = "Test Provider New"
            };
            fakeEndpoint.Providers.Add(providerDto);

            var oldProvider = new CqcProvider(providerDto)
            {
                Name = "Test Provider Old",
                CachedDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-2)) // Set cached date to 2 months ago
            };
            fakeDatastore._providers.Add(oldProvider.Id, oldProvider);

            // Test GetCqcProviderById
            var providerById = await repository.GetCqcProviderById(providerDto.ProviderId, cancellationToken: TestContext.Current.CancellationToken);
            Assert.NotNull(providerById);
            Assert.Equal(providerDto.ProviderId, providerById.Id);
            Assert.Equal(providerDto.Name, providerById.Name);

            // Check fake datastore was updated with the new provider data
            var cachedProvider = await fakeDatastore.GetProviderById(providerDto.ProviderId);
            Assert.NotNull(cachedProvider);
            Assert.Equal(providerDto.ProviderId, cachedProvider.Id);
            Assert.Equal(providerDto.Name, cachedProvider.Name);
        }
    }
}
