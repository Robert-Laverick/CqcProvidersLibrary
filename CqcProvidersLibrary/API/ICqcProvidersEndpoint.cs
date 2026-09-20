using System;
using System.Collections.Generic;
using System.Text;

namespace CqcProvidersLibrary.API
{
    public interface ICqcProvidersEndpoint
    {
        Task<IEnumerable<ProvidersResponseLineDto>> GetCqcProviders();
        Task<ProviderDto?> GetCqcProviderById(string id);
    }
}
