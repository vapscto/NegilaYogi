using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Portals.HOD
{
   public class HODExamTopper_DTO:CommonParamDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long AMST_Id { get; set; }
        public int EMCA_Id { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string AMST_FirstName { get; set; }
        public string AMST_AdmNo { get; set; }
        public decimal? ESTMP_TotalMaxMarks { get; set; }
        public decimal? ESTMP_TotalObtMarks { get; set; }
        public decimal? ESTMP_Percentage { get; set; }
        public string ELP_Flg { get; set; }
        public string ESTMP_TotalGrade { get; set; }
        public int Class_Rnk { get; set; }
        public int ESTMP_ClassRank { get; set; }
        public int ESTMP_SectionRank { get; set; }
        public string ASMC_SectionName { get; set; }
        public long ASMCL_Id { get; set; }
        public int EME_Id { get; set; }
        public string EME_ExamName { get; set; }
        public string EMCA_CategoryName { get; set; }
        public int sectionrank { get; set; }
        public long user_id { get; set; }
        public Array yearlist { get; set; }
        public Array classlist { get; set; }
        public Array exmstdlist { get; set; }
        public Array fillcategory { get; set; }
        public Array classranklist { get; set; }
        public Array sectionranklist { get; set; }
        public Array seclist { get; set; }

    }
}
