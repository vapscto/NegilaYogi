using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeCardDetailEntryDTO
    {
        public long FSFM_Id { get; set; }
        public long MI_Id { get; set; }
       // public long AMST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long FMH_Id { get; set; }
        public long FMG_Id { get; set; }
        public long FTI_Id { get; set; }
        public long FSFM_Amount { get; set; }
        public long FSFM_PaidAmount { get; set; }
        public Array fillstudent { get; set; }
        public Array fillyear { get; set; }        
        public long Amst_Id { get; set; }
        public string AMST_FirstName { get; set; }
        public string AMST_MiddleName { get; set; }
        public string AMST_LastName { get; set; }
        public string searchfilter { get; set; }
        public string FMG_GroupName { get; set; }
        public long userid { get; set; }
        public Array fillmastergroup { get; set; }
        public Array alldata { get; set; }
        public string FMH_FeeName { get; set; }
        public string FTI_Name { get; set; }
        public long FMA_Id { get; set; }
        public string multiplegroups { get; set; }
        public string ASMAY_Year { get; set; }
        public Array fillgrid { get; set; }
        public string returnval { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
    
}

