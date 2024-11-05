using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.College.Exam
{
    [Table("Exm_Col_Student_Marks_SubSubject", Schema = "CLG")]
    public  class Exm_Col_Student_Marks_SubSubjectDMO:CommonParamDMO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ECSTMSS_Id { get; set; }
        public long MI_Id { get; set; }
        public long ECSTM_Id { get; set; }
        public long ISMS_Id { get; set; }
        public int EMSS_Id { get; set; }
        public int EMSE_Id { get; set; }
        public decimal? ECSTMSS_Marks { get; set; }
        public string ECSTMSS_Grade { get; set; }
        public string ECSTMSS_MarksGradeFlg { get; set; }
        public string ECSTMSS_Flg { get; set; }
        public long Login_Id { get; set; }
        public DateTime? LoginDateTime { get; set; }
        public string IP4 { get; set; }
        public bool ECSTMSS_ActiveFlg { get; set; }

    }
}
