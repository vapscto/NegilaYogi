using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Transport
{
    public class TripDTO:CommonParamDTO
    {
        //TR_Trip.
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
        public decimal TRTP_AdvancePaid { get; set; }
       
        public Array hirerGroupList { get; set; }
        public Array vehicleList { get; set; }
        public Array driverList { get; set; }
        public Array recduparray { get; set; }
        public string TRTOB_BookingId { get; set; }
        public Array tripOnlineBookingDetails { get; set; }
        public int count { get; set; }
        public string returnVal { get; set; }
        public Array tripList { get; set; }
        public string vehicleName { get; set; }
        public string driverName { get; set; }
        public Array tripDrpwn { get; set; }
        public long asmay_id { get; set; }
        public Array hirerDrpDwn { get; set; }
        public string SearchBy { get; set; }
        public Array hirerList { get; set; }
        public string BtnClickVal { get; set; }
        public Array modeOfPaymentList { get; set; }
        public long TRMH_MobileNo { get; set; }
        public string TRMH_EmailId { get; set; }
        public Array tripDetails { get; set; }
        public Array vehicleDriverAllottmentList { get; set; }
        public long TRVDA_Id { get; set; }

        //TR_Trip_Payment
        public long TRTPP_Id { get; set; }
        public DateTime? TRTPP_ReceiptDate { get; set; }
        public string TRTPP_ReceiptNo { get; set; }
        public decimal TRTPP_TripAmount { get; set; }
        public decimal TRTPP_PaidAmount { get; set; }
        public decimal TRTPP_Discount { get; set; }
        public string TRTPP_PaymentMode { get; set; }
        public string TRTPP_ReceiptReferenceNo { get; set; }
        public long TRTPP_ChequeDDNo { get; set; }
        public DateTime? TRTPP_ChequeDDDate { get; set; }
        public bool TRTPP_ActiveFlag { get; set; }

        //TR_Hirer Opening Balance.
        public long TRHOB_Id { get; set; }
        public long TRMH_Id { get; set; }
        public decimal TRHOB_DueAmount { get; set; }
        public decimal TRHOB_ExcessAmount { get; set; }

        //TR_Approval.
        public Array approvedTripList { get; set; }
        public string TRTOB_TripStatus { get; set; }
        public Array generatedReceiptsList { get; set; }

        public TripDTO[] selectedBills { get; set; }
        public string SelectedRadioVal { get; set; }
        public TripDTO[] allottedVehicleDriver { get; set; }
        public Array bookingIdDrpDwn { get; set; }
        //Praveen
        public Array receptprint { get; set; }
        public Array instname { get; set; }
        public Array tripsheetprint { get; set; }
        public Array tripDrpwn1 { get; set; }
        public Array printbilldata { get; set; }
        public string TRTPP_BankName { get; set; }
        public Array vehicletypelist { get; set; }
        public long TRMVT_Id { get; set; }
        public long? TRTP_NoOfTravellers { get; set; }
        public string TRTP_DropLocation { get; set; }
        public long mobile { get; set; }
        public long? TRMD_MobileNo { get; set; }

    }
}
