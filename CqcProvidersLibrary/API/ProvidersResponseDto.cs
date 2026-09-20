using System;
using System.Collections.Generic;
using System.Text;

namespace CqcProvidersLibrary.API
{
    public class ProvidersResponseDto
    {
        public int Total { get; set; }
        public int Page { get; set; }
        public int PerPage { get; set; }
        public int TotalPages { get; set; }
        public string FirstPageUri { get; set; }
        public string LastPageUri { get; set; }
        public string NextPageUri { get; set; }
        public string PreviousPageUri { get; set; }
        public IEnumerable<ProvidersResponseLineDto> Providers { get; set; }
        public string Uri { get; set; }

    }
}
