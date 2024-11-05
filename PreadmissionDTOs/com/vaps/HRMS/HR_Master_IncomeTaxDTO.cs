using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class HR_Master_IncomeTaxDTO : CommonParamDTO
    {
        public long HRMIT_Id { get; set; }
        public long MI_Id { get; set; }
        public int IMFY_Id { get; set; }
        public long HRMIT_GenderFlag { get; set; }
        public string HRMIT_AgeFlag { get; set; }
        public Int32? HRMIT_FromAge { get; set; }
        public Int32? HRMIT_ToAge { get; set; }
        public bool HRMIT_Active { get; set; }
        public Array incomeTaxList { get; set; }
        public string retrunMsg { get; set; }
        public long roleId { get; set; }

        //dropdownlist
        public Array genderdropdown { get; set; }

        public Array financialYeardropdown { get; set; }

        public Array incomeTaxCessdropdown { get; set; }
        public HR_Master_IncomeTax_DetailsDTO[] incTaxDetail {get;set;}

        public long HRMITD_Id { get; set; }
        public decimal? HRMITD_AmountFrom { get; set; }
        public decimal? HRMITD_AmountTo { get; set; }
        public decimal? HRMITD_IncomeTax { get; set; }
        public Array incomeTaxDetailsList { get; set; }
    }
}
