using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.TT
{
    public class CLGCategoryMappingDTO
    {
        public long TTCC_Id { get; set; }
        public long MI_Id { get; set; }
        public long TTMC_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public bool TTCC_ActiveFlag { get; set; }
        public Array categorylist { get; set; }
        public Array savedbranch { get; set; }

        public Array acdlist { get; set; }

        public Array detailslist { get; set; }
        public Array yearlist { get; set; }
        public Array courselist { get; set; }
        public Array branchlist { get; set; }


        public BranchIDDTO[] clssids { get; set; }
        public string academicName { get; set; }
        public string className { get; set; }
        public string ASMAY_Year { get; set; }
        public string AMCO_CourseName { get; set; }
        public string AMB_BranchName { get; set; }
        public string TTMC_CategoryName { get; set; }
        public long TTCCB_Id { get; set; }
        public string categoryName { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public DateTime? CreatedDate { get; set; }

        public Array binddetails { get; set; }

        public bool returnval { get; set; }

        public string message { get; set; }

        public long userId { get; set; }

        public long roleId { get; set; }

        public string returnMsg { get; set; }



    }
    public class BranchIDDTO
    {
        public long AMB_Id { get; set; }
    }

}
