using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.com.vapstech.admission
{
    [Table("Adm_AdmissionCancel_Config")]
    public class Adm_AdmissionCancel_ConfigDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AACC_Id { get; set; }
        public long MI_Id { get; set; }
        public bool? AACC_DOAFlg { get; set; }
        public long AACC_FromDays { get; set; }
        public long AACC_ToDays { get; set; }
        public decimal? AACC_CancellationPer { get; set; }
        public bool? AACC_ActiveFlag { get; set; }
        public long AACC_CreatedBy { get; set; }
        public long AACC_UpdatedBy { get; set; }
        public DateTime? AACC_CreatedDate { get; set; }
        public DateTime? AACC_UpdatedDate { get; set; }
    }
}
