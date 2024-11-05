using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("PA_Student_Transport_Application")]
    public class PA_Student_Transport_ApplicationDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PASTA_Id { get; set; }
        public long MI_Id { get; set; }
        public long PASR_Id { get; set; }
        public long? PASTA_ApplicationNo { get; set; }
        public DateTime? PASTA_ApplicationDate { get; set; }
        public string PASTA_AreaZoneName { get; set; }
        public long? TRMA_Id { get; set; }
        public long? PASTA_PickupSMSMobileNo { get; set; }
        public long? PASTA_DropSMSMobileNo { get; set; }
        public DateTime? PASTA_PaymentDate { get; set; }
        public string PASTA_ReceiptNo { get; set; }
        public decimal PASTA_Amount { get; set; }
        public bool? PASTA_ActiveFlag { get; set; }

        public long? PASTA_PickUp_TRMR_Id { get; set; }
        public long? PASTA_PickUp_TRML_Id { get; set; }
        public long? PASTA_Drop_TRMR_Id { get; set; }
        public long? PASTA_Drop_TRML_Id { get; set; }

    }
}
