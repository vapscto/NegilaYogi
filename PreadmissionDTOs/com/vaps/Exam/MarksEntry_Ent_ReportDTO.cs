using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class MarksEntry_Ent_ReportDTO
    {
        public long EME_Id { get; set; }
        public long UserId { get; set; }
        public long ASMAY_Id { get; set; }
    
        public long Login_Id { get; set; }
        public long MI_Id { get; set; }
        public string IVRMSTAUL_UserName { get; set; }
        public exammasterDTO[] examDTO { get; set; }
        public Array stafflist { get; set; }
        public Array get_report { get; set; }
        public Array yearlist { get; set; }
        public Array subjlist { get; set; }
        public Array exmstdlist { get; set; }
        public Array exm_list { get; set; }
        public Array Studentcount { get; set; }
        public long[] staffarray { get; set; }
        public string ISMS_SubjectName { get; set; }
        public long ISMS_Id { get; set; }
        public Array SubjecList { get; set; }
        public LoginIdList[] LoginIdLists { get; set; }
        public long ApplId { get; set; }
        public string imgname { get; set; }

    }
    public class LoginIdList
    {
        public long LoginId { get; set; }
    }
  
}


