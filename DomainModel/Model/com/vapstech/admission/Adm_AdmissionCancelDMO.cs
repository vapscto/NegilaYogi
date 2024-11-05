using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.com.vapstech.admission
{
    [Table("Adm_AdmissionCancel")]
    public class Adm_AdmissionCancelDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AACA_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long? ASMS_Id { get; set; }
        public string AACA_ACReason { get; set; }
        public DateTime? AACA_ACDate { get; set; }
        public decimal? AACA_CancellationFee { get; set; }
        public decimal? AACA_ToRefundAmount { get; set; }
        public long AACA_CreatedBy { get; set; }
        public long AACA_UpdatedBy { get; set; }
        public bool? AACA_ActiveFlag { get; set; }
        public DateTime? AACA_CreatedDate { get; set; }
        public DateTime? AACA_UpdatedDate { get; set; }
    }
}
