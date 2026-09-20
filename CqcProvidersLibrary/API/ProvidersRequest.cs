using System;
using System.Collections.Generic;
using System.Text;

namespace CqcProvidersLibrary.API
{
    public class ProvidersRequest
    {
        public int PerPage { get; set; } = 100;
        public int Page { get; set; } = 1;
        public List<string> Constituency { get; set; } = [];
        public List<string> LocalAuthority { get; set; } = [];
        public List<string> InspectionDirectorate { get; set; } = [];
        public List<string> NonPrimaryInspectionCategoryCode { get; set; } = [];
        public List<string> NonPrimaryInspectionCategoryName { get; set; } = [];
        public List<string> PrimaryInspectionCategoryCode { get; set; } = [];
        public List<string> PrimaryInspectionCategoryName { get; set; } = [];
        public List<string> OverallRating { get; set; } = [];
        public List<string> Region { get; set; } = [];
        public List<string> RegulatedActivity { get; set; } = [];
        public List<string> ReportType { get; set; } = [];

        public ProvidersRequest() { }

        public List<(string Key, string Value)> GetParameters()
        {
            var queryParams = new List<(string Key, string Value)>();
            if (PerPage > 0) queryParams.Add(("perPage", PerPage.ToString()));
            if (Page > 0) queryParams.Add(("page", Page.ToString()));
            foreach( var item in Constituency )
                queryParams.Add(("constituency", item));
            foreach( var item in LocalAuthority )
                queryParams.Add(("localAuthority", item));
            foreach( var item in InspectionDirectorate )
                queryParams.Add(("inspectionDirectorate", item));
            foreach( var item in NonPrimaryInspectionCategoryCode )
                queryParams.Add(("nonPrimaryInspectionCategoryCode", item));
            foreach( var item in NonPrimaryInspectionCategoryName )
                queryParams.Add(("nonPrimaryInspectionCategoryName", item));
            foreach( var item in PrimaryInspectionCategoryCode )
                queryParams.Add(("primaryInspectionCategoryCode", item));
            foreach( var item in PrimaryInspectionCategoryName )
                queryParams.Add(("primaryInspectionCategoryName", item));
            foreach( var item in OverallRating )
                queryParams.Add(("overallRating", item));
            foreach( var item in Region )
                queryParams.Add(("region", item));
            foreach( var item in RegulatedActivity )
                queryParams.Add(("regulatedActivity", item));
            foreach( var item in ReportType )
                queryParams.Add(("reportType", item));
            return queryParams;
        }
    }
}
