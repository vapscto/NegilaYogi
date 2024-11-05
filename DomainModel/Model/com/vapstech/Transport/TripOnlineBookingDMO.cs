using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Transport
{
    [Table("TR_Trip_OnlineBooking", Schema = "TRN")]
    public class TripOnlineBookingDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TRTOB_Id { get; set; }
        public long MI_Id { get; set; }
        public DateTime? TRTOB_Date { get; set; }
        public string TRTOB_BookingId { get; set; }
        public DateTime? TRTOB_BookingDate { get; set; }
        public long TRHG_Id { get; set; }
        public string TRTOB_HirerName { get; set; }
        public string TRTOB_ConatctPerName { get; set; }
        public string TRTOB_ContactPersonDesg { get; set; }
        public long TRTOB_ContactNo { get; set; }
        public long TRTOB_MobileNo { get; set; }
        public string TRTOB_EmailId { get; set; }
        public string TRTOB_Address { get; set; }
        public string TRTOB_PickUpLocation { get; set; }
        public string TRTOB_TripAddress { get; set; }
        public DateTime? TRTOB_TripFromDate { get; set; }
        public DateTime? TRTOB_TripToDate { get; set; }
        public string TRTOB_FromTime { get; set; }
        public string TRTOB_ToTime { get; set; }
        public string TRTOB_TripStatus { get; set; }

        public bool TRTOB_ActiveFlg { get; set; }
        public decimal TRTOB_BookingAmount { get; set; }
        public string TRTOB_ModeOfPayment { get; set; }
        public string TRTOB_TripPurpose { get; set; }
        public long? TRTOB_NoOfTravellers { get; set; }
        public string TRTOB_DropLocation { get; set; }
    }
}
