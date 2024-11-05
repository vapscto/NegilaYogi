using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Transport
{
    [Table("Adm_Student_Trans_Appl_College")]
    public class CLGAdm_Std_Transport_ApplicationDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ASTACO_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long ASTACO_CurrentAY { get; set; }
        public long ASTACO_CurrentCourse { get; set; }
        public long ASTACO_CurrentBranch { get; set; }
        public long ASTACO_CurrentSemester { get; set; }
        public long ASTACO_ForAY { get; set; }
        public long ASTACO_ForSemester { get; set; }
        public string ASTACO_ApplicationNo { get; set; }
        public DateTime ASTACO_ApplicationDate { get; set; }
        public string ASTACO_AreaZoneName { get; set; }
        public long TRMA_Id { get; set; }
        public long ASTACO_PickUp_TRMR_Id { get; set; }
        public long ASTACO_PickUp_TRML_Id { get; set; }
        public long ASTACO_PickUp_TRMS_Id { get; set; }
        public long ASTACO_Drop_TRMR_Id { get; set; }
        public long ASTACO_Drop_TRML_Id { get; set; }
        public long ASTACO_Drop_TRMS_Id { get; set; }
        public long ASTACO_PickupSMSMobileNo { get; set; }
        public long ASTACO_DropSMSMobileNo { get; set; }
        public string ASTACO_ApplStatus { get; set; }
        public string ASTACO_PaymentMade { get; set; }
        public string ASTACO_ReceiptNo { get; set; }
        public decimal ASTACO_Amount { get; set; }
        public bool ASTACO_ActiveFlag { get; set; }
        public string ASTACO_Landmark { get; set; }
        public string ASTACO_Regnew { get; set; }
        public long? ASTACO_Phoneoff { get; set; }
        public long? ASTACO_PhoneRes { get; set; }
        public string ASTACO_Remarks { get; set; }

    }
}
