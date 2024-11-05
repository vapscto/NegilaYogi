using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.com.vapstech.Transport
{
    [Table("TR_Configuration", Schema = "TRN")]
    public class TransportStandardsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TRC_Id { get; set; }
        public long MI_Id { get; set; }
        public int TRC_DLExpReminderDays { get; set; }
        public int TRC_EmmisionExpMonths { get; set; }
        public int TRC_EmmisionExpDays { get; set; }
        public int TRC_TaxExpMonths { get; set; }
        public int TRC_TaxExpDays { get; set; }
        public int TRC_FitnessExpMonths { get; set; }
        public int TRC_FitnessExpDays { get; set; }
        public int TRC_SpeedControlMonths { get; set; }
        public int TRC_SpeedControlDays { get; set; }
        public int TRC_PermitMonths { get; set; }
        public int TRC_PermitDays { get; set; }
        public int TRC_CeaseFireMonths { get; set; }
        public int TRC_CeaseFireDays { get; set; }
        public int TRC_InsuranceMonths { get; set; }
        public int TRC_InsuranceDays { get; set; }
        public int TRC_GreenTaxMonths { get; set; }
        public int TRC_GreenTaxDays { get; set; }

    }
}
