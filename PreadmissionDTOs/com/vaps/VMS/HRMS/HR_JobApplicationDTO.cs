using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.VMS.HRMS
{
    public class HR_JobApplicationDTO : CommonParamDTO
    {
        public long HRJA_Id { get; set; }
        public string HRJA_FirstName { get; set; }
        public string HRJA_LastName { get; set; }
        public string HRJA_EmailId { get; set; }
        public long HRJA_MobileNo { get; set; }
        public string HRJA_Gender { get; set; }
        public DateTime HRJA_DOB { get; set; }
        public string HRJA_CurrentLocation { get; set; }
        public string HRJA_Qualification { get; set; }
        public string HRJA_CurrentCompany { get; set; }
        public decimal HRJA_TotalExpFrom { get; set; }
        public decimal HRJA_TotalExpTo { get; set; }
        public decimal HRJA_CurrentCTCPerYear { get; set; }
        public decimal HRJA_ExpectedCTCPerYear { get; set; }
        public long HRJA_NoticePeriod { get; set; }
        public string HRJA_Resume { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }
    }

}