using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.TT
{
    public class CLGConsecutiveDTO
    {
        public bool returnval;
        public string returnduplicatestatus;

        public long TTCC_Id { get; set; }
        public long MI_Id { get; set; }
        public long TTMC_Id { get; set; }
        public long ACMS_Id { get; set; }
 
        public long HRME_Id { get; set; }
        public long ISMS_Id { get; set; }
        public decimal TTCC_NoOfPeriods { get; set; }
        public decimal TTCC_AllotPeriods { get; set; }
        public decimal TTCC_RemPeriods { get; set; }
        public decimal TTCC_NoOfConPeriods { get; set; }
        public decimal TTCC_NoOfConDays { get; set; }
        public int TTCC_BefAftApplFlag { get; set; }
        public int TTCC_BefAftFalg { get; set; }
        public decimal TTCC_BefAftPeriod { get; set; }
        public string TTCC_AllotedFlag { get; set; }
        public long ASMAY_Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Array consecutivelst { get; set; }
        public Array stafflist { get; set; }
        public Array subjectlist { get; set; }
        public Array consecutivelstedit { get; set; }
        public Array catelist { get; set; }
        public Array academiclist { get; set; }
        public Array courselist { get; set; }
        public Array sectionlist { get; set; }
        public Array branchlist { get; set; }
        public Array semisterlist { get; set; }

        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }

        public string academicyr { get; set; }

        public string AMB_BranchName { get; set; }
        public string AMCO_CourseName { get; set; }
        public string AMSE_SEMName { get; set; }
        public string ASMAY_Year { get; set; }
        public Array staffDrpDwn { get; set; }
        public Array ttsujectslist { get; set; }
        public Array subjDrpDwn { get; set; }
        public bool TTCC_ActiveFlag { get; set; }
        public string ASMAYYear { get; set; }
        public string CategoryName { get; set; }
        public decimal NoOfPeriods { get; set; }
       
        public string SectionName { get; set; }
        public string staffName { get; set; }
        public string SubjectName { get; set; }
        public Array classbycategory { get; set; }
      
        public string TTMC_CategoryName { get; set; }     
        public string staffNamelst { get; set; }
     
     
    }
}
