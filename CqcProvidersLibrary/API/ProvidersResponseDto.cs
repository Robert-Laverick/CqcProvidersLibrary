using System;
using System.Collections.Generic;
using System.Text;

namespace CqcProvidersLibrary.API
{
    public class ProvidersResponseDto
    {
        public required int Total { get; set; }
        public required int Page { get; set; }
        public required int PerPage { get; set; }
        public required int TotalPages { get; set; }
        public required string FirstPageUri { get; set; }
        public required string LastPageUri { get; set; }
        public required string NextPageUri { get; set; }
        public required string PreviousPageUri { get; set; }
        public required IEnumerable<ProvidersResponseLineDto> Providers { get; set; }
        public required string Uri { get; set; }

    }
}
