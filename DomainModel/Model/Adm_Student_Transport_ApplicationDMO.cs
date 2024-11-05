using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Adm_Student_Transport_Application")]
    public class Adm_Student_Transport_ApplicationDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long ASTA_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public string ASTA_ApplicationNo { get; set; }
        public DateTime? ASTA_ApplicationDate { get; set; }
        public string ASTA_AreaZoneName { get; set; }
        public long TRMA_Id { get; set; }
        public long? ASTA_FatherMobileNo { get; set; }
        public long? ASTA_MotherMobileNo { get; set; }
        public string ASTA_ApplStatus { get; set; }
        public DateTime? ASTA_PaymentDate { get; set; }
        public string ASTA_ReceiptNo { get; set; }
        public decimal ASTA_Amount { get; set; }
        public bool ASTA_ActiveFlag { get; set; }
        public long ASTA_PickUp_TRMR_Id { get; set; }
        public long ASTA_PickUp_TRML_Id { get; set; }
        public long ASTA_Drop_TRMR_Id { get; set; }
        public long ASTA_Drop_TRML_Id { get; set; }

        public long ASTA_CurrentAY { get; set; }
        public long ASTA_CurrentClass { get; set; }
        public long ASTA_FutureAY { get; set; }
        public long ASTA_FutureClass { get; set; }

        public string ASTA_Landmark { get; set; }

        public string ASTA_Regnew  { get; set; }

        public long? ASTA_Phoneoff { get; set; }

        public long? ASTA_PhoneRes  { get; set; }

        public string ASTA_Remarks { get; set; }
        public long? ASTA_PickUp_TRMS_Id { get; set; }
        public long? ASTA_Drop_TRMS_Id { get; set; }

    }
}
