using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using Microsoft.Data.SqlClient;

namespace CqcProvidersLibrary.Repository
{
    public class CqcProvidersDatastore: ICqcProvidersDatastore
    {
        private readonly string _connectionString;

        public CqcProvidersDatastore(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task InsertOrUpdateProvider(CqcProvider provider)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();

            try
            {
                //Delete Locations
                const string deleteLocationsSql = "DELETE FROM ProviderLocations WHERE ProviderId = @ProviderId";
                await connection.ExecuteAsync(deleteLocationsSql, new { ProviderId = provider.Id }, transaction);


                const string deleteSql = "DELETE FROM Providers WHERE Id = @ProviderId";
                await connection.ExecuteAsync(deleteSql, new { ProviderId = provider.Id }, transaction);

                const string insertProviderSql = @"INSERT INTO Providers
                   ([Id]
                   ,[OrganisationType]
                   ,[OwnershipType]
                   ,[Type]
                   ,[Name]
                   ,[BrandId]
                   ,[BrandName]
                   ,[RegistrationStatus]
                   ,[RegistrationDate]
                   ,[CompaniesHouseNumber]
                   ,[CharityNumber]
                   ,[Website]
                   ,[PostalAddressLine1]
                   ,[PostalAddressLine2]
                   ,[PostalAddressTownCity]
                   ,[PostalAddressCounty]
                   ,[Region]
                   ,[PostalCode]
                   ,[Uprn]
                   ,[OnspdLatitude]
                   ,[OnspdLongitude]
                   ,[MainPhoneNumber]
                   ,[InspectionDirectorate]
                   ,[Constituency]
                   ,[LocalAuthority]
                   ,[LastInspectionDate]
                   ,[CachedDate])
             VALUES
                   (@Id
                   ,@OrganisationType
                   ,@OwnershipType
                   ,@Type
                   ,@Name
                   ,@BrandId
                   ,@BrandName
                   ,@RegistrationStatus
                   ,@RegistrationDate
                   ,@CompaniesHouseNumber
                   ,@CharityNumber
                   ,@Website
                   ,@PostalAddressLine1
                   ,@PostalAddressLine2
                   ,@PostalAddressTownCity
                   ,@PostalAddressCounty
                   ,@Region
                   ,@PostalCode
                   ,@Uprn
                   ,@OnspdLatitude
                   ,@OnspdLongitude
                   ,@MainPhoneNumber
                   ,@InspectionDirectorate
                   ,@Constituency
                   ,@LocalAuthority
                   ,@LastInspectionDate
                   ,@CachedDate)";
                await connection.ExecuteAsync(insertProviderSql, provider, transaction);

                //Insert Locations
                const string insertLocationSql = @"INSERT INTO ProviderLocations
                           ([ProviderId] ,[LocationId])
                     VALUES
                           (@ProviderId ,@LocationId)";

                foreach (var location in provider.LocationIds)
                {
                    await connection.ExecuteAsync(insertLocationSql, new { ProviderId = provider.Id, LocationId = location }, transaction);
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<CqcProvider?> GetProviderById(string id)
        {
            const string sql = "SELECT * FROM Providers WHERE Id = @Id";

            using var connection = new SqlConnection(_connectionString);
            var provider = await connection.QuerySingleOrDefaultAsync<CqcProvider>(sql, new { Id = id });

            if (provider != null)
            {
                const string locationSql = "SELECT LocationId FROM ProviderLocations WHERE ProviderId = @ProviderId";
                var locationIds = await connection.QueryAsync<string>(locationSql, new { ProviderId = id });
                provider.LocationIds = locationIds.ToList();
            }

            return provider;
        }
    }
}
