using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.College.Exam
{
    [Table("Exm_Col_Studentwise_Subjects", Schema = "CLG")]
    public class Exm_Col_Studentwise_SubjectsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
    }
}
