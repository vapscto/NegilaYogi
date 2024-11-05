using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Transport
{
    public class TripOnlineBookingDTO:CommonParamDTO
    {
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
        public long TRMH_Id { get; set; }
        public Array hirerGroupList { get; set; }
        public Array hirerDrpDwn { get; set; }
        public Array hirerDetails { get;set;}
        public long asmay_id { get; set; }
        public Array tripOnlineBookingList { get; set; }
        public int count { get; set; }
        public string returnVal { get; set; }
        public string trhG_HirerGroup { get; set; }
        public decimal TRTOB_BookingAmount { get; set; }
        public string TRTOB_ModeOfPayment { get; set; }
        public Array modeOfPaymentList { get; set; }
        public long TRTPP_ChequeDDNo { get; set; }
        public DateTime? TRTPP_ChequeDDDate { get; set; }

        public decimal dueamount { get; set; }
        public decimal excessamount { get; set; }
        public string IsOnline { get; set; }
        public Array editDataList { get; set; }
        public string TRTOB_TripPurpose { get; set; }
        public string TRTOB_DropLocation { get; set; }
        public long? TRTOB_NoOfTravellers { get; set; }

    }
}
