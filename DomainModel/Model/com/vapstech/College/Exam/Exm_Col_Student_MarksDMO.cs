using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.College.Exam
{
    [Table("Exm_Col_Student_Marks", Schema = "CLG")]
    public class Exm_Col_Student_MarksDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ECSTM_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMCO_ID { get; set; }
        public long AMB_ID { get; set; }
        public long AMSE_ID { get; set; }
        public long ACMS_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public int EME_Id { get; set; }
        public long ISMS_Id { get; set; }
        public decimal? ECSTM_Marks { get; set; }
        public string ECSTM_Grade { get; set; }
        public string ECSTM_MarksGradeFlg { get; set; }
        public string ECSTM_Flg { get; set; }
        public long Login_Id { get; set; }
        public DateTime? ECSTM_LoginDate { get; set; }
        public string ECSTM_IP4Address { get; set; }
        public bool ECSTM_ActiveFlg { get; set; }
        public long? ECSTM_CreatedBy { get; set; }
        public long? ECSTM_UpdatedBy { get; set; }
        public List<Exm_Col_Student_Marks_SubSubjectDMO> Exm_Col_Student_Marks_SubSubjectDMO { get; set; }

    }
}
