using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class Promotion_Marks_UpdateDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long EMCA_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long ISMS_Id { get; set; }
        public Array yearlist { get; set; }
        public Array categorylist { get; set; }
        public Array classlist { get; set; }
        public Array sectionlist { get; set; }
        public Array subjectlist { get; set; }
        public Array prom_subj_groups { get; set; }
        public Array prom_students { get; set; }
        public Array prom_subjmarks { get; set; }
        public Array prom_subj_groupmarks { get; set; }
        public promo_stu_marksDTO[] promo_stu_marks { get; set; }
    }
    public class promo_stu_marksDTO
    {
        public int AMST_Id { get; set; }
        public int AMST_FirstName { get; set; }
        public int AMST_AdmNo { get; set; }
        public int AMAY_RollNo { get; set; }
        public int AMST_DOB { get; set; }
        public int AMST_Photoname { get; set; }
        public promo_stu_grp_marksDTO[] promo_stu_grp_marks { get; set; }
        public int ESTMPPS_Id { get; set; }
        public decimal? ESTMPPS_ObtainedMarks { get; set; }
    }
    public class promo_stu_grp_marksDTO
    {
        public int ESTMPPS_Id { get; set; }
        public int EMPSG_Id { get; set; }
        public decimal? ESTMPPSG_GroupObtMarks { get; set; }
    }

}
