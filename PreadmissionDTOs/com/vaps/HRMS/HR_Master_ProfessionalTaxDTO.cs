using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class HR_Master_ProfessionalTaxDTO:CommonParamDTO
    {
        public long HRMPT_Id { get; set; }
        public long MI_Id { get; set; }
        public decimal? HRMPT_SalaryFrom { get; set; }
        public decimal? HRMPT_SalaryTo { get; set; }
        public decimal? HRMPT_PTax { get; set; }
        public bool HRMPT_ActiveFlag { get; set; }
        public Array pTaxrList { get; set; }
        public string retrunMsg { get; set; }
        public long roleId { get; set; }
    }
}
