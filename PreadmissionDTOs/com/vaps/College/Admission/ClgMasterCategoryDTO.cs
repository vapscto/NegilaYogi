using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.College.Admission
{
    public class ClgMasterCategoryDTO
    {
        public long AMCOC_Id { get; set; }

        public long MI_Id { get; set; }

        public string AMCOC_Name { get; set; }
        public string AMCOC_Address { get; set; }

        public string AMCOC_Details { get; set; }

        public string AMCOC_Type { get; set; }

        public string ACMC_RegNoPrefix { get; set; }

        //public string returnval { get; set; }

        public Array categoryList { get; set; }
        public Array institutionDrpdwn { get; set; }

        public Array masterCategoryList { get; set; }
    
            public string message { get; set; }
        public bool returnval { get; set; }
    }
}
