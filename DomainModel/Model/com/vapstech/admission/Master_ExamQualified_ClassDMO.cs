using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.admission
{
    [Table("Ivrm_Master_QualifiedClass")]
  public  class Master_ExamQualified_ClassDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IMQC_Id { get; set; }
        public string IMQC_ExamName { get; set; }
        public long MI_ID { get; set; }
        public DateTime IMQC_CreatedDate { get; set; }
        public DateTime IMQC_UpdatedDate { get; set; }
        public long IMQC_CreatedBy { get; set; }
        public long IMQC_UpdatedBy { get; set; }
        public bool IMQC_ActiveFlag { get; set; }

    }
}
