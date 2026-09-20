using System;
using System.Collections.Generic;
using System.Text;

namespace CqcProvidersLibrary.API
{
    public interface ICqcProvidersEndpoint
    {
        Task<ProvidersResponseDto?> GetCqcProviders(ProvidersRequest request);
        Task<ProviderDto?> GetCqcProviderById(string id);
    }
}
