using CqcProvidersLibrary.API;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace CqcProvidersLibrary.Repository
{
    public class CqcProvider
    {
        public required string Id { get; set; }
        public IEnumerable<string> LocationIds { get; set; } = new List<string>();
        public string? OrganisationType { get; set; }
        public string? OwnershipType { get; set; }
        public string? Type { get; set; }
        public string? Name { get; set; }
        public string? BrandId { get; set; }
        public string? BrandName { get; set; }
        public string? RegistrationStatus { get; set; }
        public DateOnly? RegistrationDate { get; set; }
        public string? CompaniesHouseNumber { get; set; }
        public string? CharityNumber { get; set; }
        public string? Website { get; set; }
        public string? PostalAddressLine1 { get; set; }
        public string? PostalAddressLine2 { get; set; }
        public string? PostalAddressTownCity { get; set; }
        public string? PostalAddressCounty { get; set; }
        public string? Region { get; set; }
        public string? PostalCode { get; set; }
        public string? Uprn { get; set; }
        public double? OnspdLatitude { get; set; }
        public double? OnspdLongitude { get; set; }
        public string? MainPhoneNumber { get; set; }
        public string? InspectionDirectorate { get; set; }
        public string? Constituency { get; set; }
        public string? LocalAuthority { get; set; }
        public DateOnly? LastInspectionDate { get; set; }
        public DateOnly CachedDate { get; set; }

        public CqcProvider() { 
            
        }

        [SetsRequiredMembers]
        public CqcProvider(ProviderDto dto)
        {
            Id = dto.ProviderId;
            LocationIds = dto.LocationIds;
            OrganisationType = dto.OrganisationType;
            OwnershipType = dto.OwnershipType;
            Type = dto.Type;
            Name = dto.Name;
            BrandId = dto.BrandId;
            BrandName = dto.BrandName;
            RegistrationStatus = dto.RegistrationStatus;
            RegistrationDate = dto.RegistrationDate;
            CompaniesHouseNumber = dto.CompaniesHouseNumber;
            CharityNumber = dto.CharityNumber;
            Website = dto.Website;
            PostalAddressLine1 = dto.PostalAddressLine1;
            PostalAddressLine2 = dto.PostalAddressLine2;
            PostalAddressTownCity = dto.PostalAddressTownCity;
            PostalAddressCounty = dto.PostalAddressCounty;
            Region = dto.Region;
            PostalCode = dto.PostalCode;
            Uprn = dto.Uprn;
            OnspdLatitude = dto.OnspdLatitude;
            OnspdLongitude = dto.OnspdLongitude;
            MainPhoneNumber = dto.MainPhoneNumber;
            InspectionDirectorate = dto.InspectionDirectorate;
            Constituency = dto.Constituency;
            LocalAuthority = dto.LocalAuthority;
            if (dto.LastInspection != null)
            {
                LastInspectionDate = dto.LastInspection.Date;
            }
            CachedDate = DateOnly.FromDateTime(DateTime.UtcNow);
        }
    }
}
