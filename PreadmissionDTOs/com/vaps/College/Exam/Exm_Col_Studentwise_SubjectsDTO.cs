using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Exam
{
    public class Exm_Col_Studentwise_SubjectsDTO
    {
        public long ECSTSU_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long ACMS_Id { get; set; }
        public int EMG_Id { get; set; }
        public long ISMS_Id { get; set; }
        public bool ECSTSU_ElectiveFlag { get; set; }
        public bool ECSTSU_ActiveFlg { get; set; }
        public Array courseslist { get; set; }
        public Array sections { get; set; }
        public Array subjectgrplist { get; set; }
        public Array branchlist { get; set; }
        public Array yearlist { get; set; }
        public Array semisters { get; set; }
        public Array studlist { get; set; }
        public Array allsubject_list { get; set; }
        public Array allstudent_details { get; set; }
        public string AMCST_Name { get; set; }
        public string AMCST_AdmNo { get; set; }
        public string AMCST_RegistrationNo { get; set; }
        public string ISMS_SubjectName { get; set; }
        public tempDTO[] get_list { get; set; }
        public bool returnval { get; set; }
        public string returnduplicatestatus { get; set; }
    }
    public class tempDTO
    {
        public long amcsT_Id { get; set; }
        public string amcsT_Name { get; set; }
        public tempDTO1[] sub_list { get; set; }
    }
    public class tempDTO1
    {
        public long id { get; set; }
        public string name { get; set; }
    }
}
