using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Transport
{
    [Table("TR_Trip", Schema ="TRN")]
    public class TripDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TRTP_Id { get; set; }
        public long MI_Id { get; set; }
        public DateTime? TRTP_BookingDate { get; set; }
        public DateTime? TRTP_TripDate { get; set; }

        public long TRHG_Id { get; set; }
        public long TRMV_Id { get; set; }
        public string TRTP_PickUpLocation { get; set; }
        public string TRTP_TripAddress { get; set; }

        public long TRVD_Id { get; set; }
        public string TRTP_FromTime { get; set; }
        public string TRTP_ToTime { get; set; }
        public long TRTP_OpeningKM { get; set; }

        public long TRTP_ClosingKM { get; set; }
        public string TRVD_TripId { get; set; }
        public bool TRTP_BillGeneratedFlag { get; set; }
        public string TRTP_BillNo { get; set; }

        public DateTime? TRTP_BillDate { get; set; }
        public decimal TRTP_BillAmount { get; set; }
        public decimal TRTP_DiscountAmount { get; set; }

        public decimal TRTP_PaidAmount { get; set; }
        public decimal TRTP_BalanceAmount { get; set; }
        public bool TRTP_ActiveFlg { get; set; }
        public decimal TRTP_AdvanceReceived { get; set; }
        public decimal TRTP_AdvancePaid { get; set; }

        public decimal TRTP_TotalReceivable { get; set; }
        public long TRTOB_Id { get; set; }
        public string TRTP_HirerName { get; set; }
        public long TRTP_HirerContactNo { get; set; }

        public string TRTP_HirerContactPerson { get; set; }
        public string TRTP_HirerAddress { get; set; }
        public DateTime? TRTP_TripFromDate { get; set; }
        public DateTime? TRTP_TripToDate { get; set; }
        public decimal TRTP_AdjustedWithExcess { get; set; }
        public string TRTP_ModeOfPayment { get; set; }
        public string TRTP_DropLocation { get; set; }
        public long? TRTP_NoOfTravellers { get; set; }
        public List<TRVehicleDriverAllottmentDMO> TRVehicleDriverAllottmentDMO { get; set; }
    }
}
