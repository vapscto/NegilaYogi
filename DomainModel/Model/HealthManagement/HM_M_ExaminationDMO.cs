using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.HealthManagement
{
    [Table("HM_M_Examination", Schema = "HM")]
    public class HM_M_ExaminationDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HMMEXM_Id { get; set; }
        public long MI_Id { get; set; }
        public string HMMEXM_ExaminationName { get; set; }
        public string HMMEXM_ExamDesc { get; set; }
        public bool HMMEXM_ActiveFlg { get; set; }
        public DateTime? HMMEXM_CreatedDate { get; set; }
        public DateTime? HMMEXM_UpdatedDate { get; set; }
        public long HMMEXM_CreatedBy { get; set; }
        public long HMMEXM_UpdatedBy { get; set; }
    }
}
