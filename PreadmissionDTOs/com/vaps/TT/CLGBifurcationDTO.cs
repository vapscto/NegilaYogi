using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.TT
{
    public class CLGBifurcationDTO
    {
        public long TTB_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long TTMC_Id { get; set; }
        public string TTB_BifurcationName { get; set; }      
        public int TTB_NoOfPeriods { get; set; }
        public int TTB_AllotPeriods { get; set; }
        public int TTB_RemPeriods { get; set; }
        public int TTB_ConsecutiveFlag { get; set; }
        public int TTB_NoOfConPeriods { get; set; }
        public int TTB_NoOfConDays { get; set; }
        public int TTB_BefAftApplFlag { get; set; }
        public string TTB_BefAftFalg { get; set; }
        public int TTMP_Id { get; set; }
        public string TTB_AllotedFlag { get; set; }
        public bool TTB_ActiveFlag { get; set; }
   
        public long userId { get; set; }
        public long roleId { get; set; }
        public Array categorylist { get; set; }
        public Array acdlist { get; set; }
        public Array detailslist { get; set; }
        public Array editdetailslist { get; set; }
        public Array classlist { get; set; }
        public Array sectionlist { get; set; }
        public Array subjectlist { get; set; }
        public Array periodlist { get; set; }
        public Array stafflist { get; set; }
        public CLGBifurcationDTO[] combinationlist { get; set; }
        public string returnMsg { get; set; }
        public string AcdYear { get; set; }
        public string periodname { get; set; }
        public string className { get; set; }
        public string categoryName { get; set; }
        public string sectioname { get; set; }
        public string staffname { get; set; }
        public string subjectname { get; set;}
        public string bifricationName { get; set; }
        public long TTBD_Id { get; set; }
        public Array viewdata { get; set; }
        public string returnduplicatestatus { get; set; }
        public long TTMD_Id { get; set; }
        public long ACMS_Id { get; set; }
        public long HRME_Id { get; set; }
        public long ISMS_Id { get; set; }
        
        public bool TTMD_ActiveFlag { get; set; }

        public long Order_Id { get; set; }
    

        public bool returnval { get; set; }

        public Array Daylist { get; set; }
     
        public Array yearlist { get; set; }
        public Array courselist { get; set; }
        public Array Daydetailedit { get; set; }

        public Array Daylistedit { get; set; }
        public Array branchlist { get; set; }
  
        public CLGBifurcationDTO[] dayids { get; set; }
        public CLGBifurcationDTO[] semids { get; set; }

        public Array Daylist_class { get; set; }
        public int day_count { get; set; }

        public long TTMDC_Id { get; set; }
    
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }

        public string academicyr { get; set; }
     
        public string AMB_BranchName { get; set; }
        public string AMCO_CourseName { get; set; }
        public string AMSE_SEMName { get; set; }
        public string ASMAY_Year { get; set; }
        public string catgname { get; set; }
       

    }
}
