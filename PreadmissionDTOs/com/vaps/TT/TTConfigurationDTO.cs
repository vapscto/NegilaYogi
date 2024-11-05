using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.TT
{
    public class TTConfigurationDTO:CommonParamDTO
    {
        public long TTC_Id { get; set; }
        public long MI_Id { get; set; }
        public bool TTC_StaffwiseContiniousPeriods { get; set; }
        public bool TTC_CTConstraintFlg { get; set; }
        public int TTC_CTConstraintNoOfDays { get; set; }
        public bool TTC_MaxMinPeriodCheckingFlg { get; set; }

        public Array Detailslist { get; set; }     

        public bool returnval { get; set; }
        public string returnduplicatestatus { get; set; }
        public long ASMCL_ID { get; set; }
        public long ASMS_ID { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }

        public long HRME_Id { get; set; }
        public string TTMSAB_Abbreviation { get; set; }
    }
}
