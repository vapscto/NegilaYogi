using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.admission
{
    [Table("Adm_Student_Complaints")]
   public class StudentCompliants_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ASCOMP_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public DateTime ASCOMP_Date { get; set; }
        public string ASCOMP_Complaints { get; set; }
        public string ASCOMP_Subject { get; set; }
        public string ASCOMP_FileName { get; set; }
        public string ASCOMP_FilePath { get; set; }
        public long? ASCOMP_ComplaintsBy { get; set; }
        public bool? ASCOMP_ActiveFlg { get; set; }
        public long? ASCOMP_CreatedBy { get; set; }
        public DateTime? ASCOMP_CreatedDate { get; set; }
        public long? ASCOMP_UpdatedBy { get; set; }
        public DateTime? ASCOMP_UpdatedDate { get; set; }
    }
}
