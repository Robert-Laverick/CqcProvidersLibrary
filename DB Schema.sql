CREATE TABLE [dbo].[Providers](
	[Id] [nvarchar](50) NOT NULL,
	[OrganisationType] [nvarchar](500) NULL,
	[OwnershipType] [nvarchar](500) NULL,
	[Type] [nvarchar](500) NULL,
	[Name] [nvarchar](500) NULL,
	[BrandId] [nvarchar](500) NULL,
	[BrandName] [nvarchar](500) NULL,
	[RegistrationStatus] [nvarchar](500) NULL,
	[RegistrationDate] [date] NULL,
	[CompaniesHouseNumber] [nvarchar](500) NULL,
	[CharityNumber] [nvarchar](500) NULL,
	[Website] [nvarchar](500) NULL,
	[PostalAddressLine1] [nvarchar](500) NULL,
	[PostalAddressLine2] [nvarchar](500) NULL,
	[PostalAddressTownCity] [nvarchar](500) NULL,
	[PostalAddressCounty] [nvarchar](500) NULL,
	[Region] [nvarchar](500) NULL,
	[PostalCode] [nvarchar](500) NULL,
	[Uprn] [nvarchar](500) NULL,
	[OnspdLatitude] [float] NULL,
	[OnspdLongitude] [float] NULL,
	[MainPhoneNumber] [nvarchar](500) NULL,
	[InspectionDirectorate] [nvarchar](500) NULL,
	[Constituency] [nvarchar](500) NULL,
	[LocalAuthority] [nvarchar](500) NULL,
	[LastInspectionDate] [date] NULL,
	[CachedDate] [date] NOT NULL,
	CONSTRAINT [PK_Providers] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)
)

CREATE TABLE [dbo].[ProviderLocations](
	[ProviderId] [nvarchar](50) NOT NULL,
	[LocationId] [nvarchar](50) NOT NULL,
	CONSTRAINT [PK_ProviderLocations] PRIMARY KEY CLUSTERED 
	(
		[ProviderId] ASC,
		[LocationId] ASC
	)
)