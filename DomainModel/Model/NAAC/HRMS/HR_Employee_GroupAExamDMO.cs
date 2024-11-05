using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.HRMS
{
    [Table("HR_Employee_GroupAExam")]
    public class HR_Employee_GroupAExamDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HREMGAE_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public long HRMEGA_Id { get; set; }
        public long HREMGAE_Year { get; set; }
        public string HREMGAE_GPFlg { get; set; }
        public string HREMGAE_Marks { get; set; }
        public bool HREMGAE_ActiveFlg { get; set; }
        public long HREMGAE_CreatedBy { get; set; }
        public long HREMGAE_UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string HREMGAE_SubjectName { get; set; }
    }
}
