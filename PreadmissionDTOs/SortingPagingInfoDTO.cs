using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class SortingPagingInfoDTO : CommonParamDTO
    {
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public int CurrentPageIndex { get; set; }
        public int TotalItems { get; set; }
        public string searchString { get; set; }
        public string sortOrder { get; set; }
        public string searchType { get; set; }
    }
}
