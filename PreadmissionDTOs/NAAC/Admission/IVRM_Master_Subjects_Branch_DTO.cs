using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission
{
   public class IVRM_Master_Subjects_Branch_DTO:CommonParamDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long IMSBR_Id { get; set; }
        public long ISMS_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public bool IMSBR_ActiveFlg { get; set; }
        public string AMCO_CourseName { get; set; }
        public string AMB_BranchName { get; set; }
        public string ISMS_SubjectName { get; set; }


        public bool returnval { get; set; }
        public Array alldata { get; set; }
        public Array editlist { get; set; }
        public Array cousrselist { get; set; }
        public Array branchlist { get; set; }
        public Array subjectlist { get; set; }

        public bool duplicate { get; set; }


    }
}
