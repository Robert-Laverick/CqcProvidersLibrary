using System;
using System.Collections.Generic;
using System.Text;

namespace CqcProvidersLibrary.Repository
{
    public interface ICqcProvidersDatastore
    {
        Task InsertOrUpdateProvider(CqcProvider provider);
        Task<CqcProvider?> GetProviderById(string id);
    }
}
