using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.University
{
  public  class MC_342_HRIncentivesDTO
    {
        public long NCMCHRI342_Id { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long ASMAY_Id { get; set; }
        public long NCMCHRI342_Year { get; set; }
        public bool NCMCHRI342_CareerAdvancement { get; set; }
        public bool NCMCHRI342_IncrementInSalary { get; set; }
        public bool NCMCHRI342_RecThroughWebsiteNotification { get; set; }
        public bool NCMCHRI342_CommnCertAndCashaward { get; set; }
        public bool NCMCHRI342_ActiveFlag { get; set; }
        public long NCMCHRI342_CreatedBy { get; set; }
        public long NCMCHRI342_UpdatedBy { get; set; }
        public DateTime? NCMCHRI342_CreatedDate { get; set; }
        public DateTime? NCMCHRI342_UpdatedDate { get; set; }

        public Array allacademicyear { get; set; }
        public Array alldata1 { get; set; }
        public Array institutionlist { get; set; }
        public string ASMAY_Year { get; set; }       
        public bool duplicate { get; set; }       
        public string msg { get; set; }
        public bool returnval { get; set; }
        public Array editlist { get; set; }    
       
        public Array yearlist { get; set; }
    


    }
}
