using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Transport
{
    public class ServiceStationDTO
    {
        public long TRMSST_Id { get; set; }
        public long MI_Id { get; set; }
        public long USER_Id { get; set; }
        public string TRMSST_ServiceStationName { get; set; }
        public long TRMSST_ContactNo { get; set; }
        public string TRMSST_EmailId { get; set; }
        public string TRMSST_Address { get; set; }
        public bool TRMSST_ActiveFlag { get; set; }
        public bool statuscount { get; set; }
       
        public long TRPAP_Id { get; set; }
        public long TRMV_Id { get; set; }
        public long TRMD_Id { get; set; }
        public string TRMSES_Parts { get; set; }
        public DateTime TRMSES_Date { get; set; }
        public Array payingdatalist { get; set; }
        public Array requisitionlist { get; set; }
        public Array instlist { get; set; }
        public Array modeOfPaymentList { get; set; }
        public Array servnamelist { get; set; }
        public Array itemlist { get; set; }
        public Array servnamegrid { get; set; }
        public Array partlist { get; set; }
        public Array serivelist { get; set; }
        public string returnVal { get; set; }
        public Array editDataList { get; set; }
        public Array servicenolist { get; set; }
        public bool retval { get; set; }

        public string TRMV_VehicleNo { get; set; }
        public string TRMD_DriverName { get; set; }
        public Array vehicaldata { get; set; }
        public Array paymentlist { get; set; }
        public Array driverdata { get; set; }
        public Array editparts { get; set; }
        public bool TRMSESP_ActiveFlag { get; set; }
        public Array fillvahicletype { get; set; }
        public Array getloaddata { get; set; }
        public long TRMVT_Id { get; set; }
        public DateTime TODATE { get; set; }
        public DateTime FRMDATE { get; set; }


        public long TRPAPT_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string TRPAPT_PType { get; set; }
        public bool TRPAPT_ActiveFlag { get; set; }
        public Array parttypegrig { get; set; }
        public Array parttypedropdown { get; set; }
        public Array requisitionlistold { get; set; }
        public vehicleid[] vhlid { get; set; }
        public long TRSE_Id { get; set; }
        public string TRSE_ServiceStationName { get; set; }
      
        public string TRSE_ServiceRefNo { get; set; }
        public string TRSE_ProblemsListed { get; set; }
        public string TRSE_ServiceDetails { get; set; }
        public string TRSE_BillNo { get; set; }
        public decimal TRSE_LabourCharges { get; set; }
        public decimal TRSE_ItemsCost { get; set; }
        public DateTime? TRSE_BillDate { get; set; }
        public DateTime TRSE_ServiceDate { get; set; }
        public decimal TRSE_TotalBillValue { get; set; }
        public decimal TRSE_TotalDiscount { get; set; }
        public decimal TRSE_TaxValue { get; set; }
        public decimal TRSE_TDSValue { get; set; }
        public decimal TRSE_TotalPaid { get; set; }
        public bool TRSE_ActiveFlag { get; set; }
        public long TRSED_Id { get; set; }
      
        public string TRSED_ItemsName { get; set; }
        public decimal TRSED_Qty { get; set; }
        public string TRSED_Remarks { get; set; }
        public string TRSED_ProblemsListed { get; set; }
        public string TRSED_ServiceDetails { get; set; }
        public decimal TRSED_Amount { get; set; }
        public decimal TRSED_TotalDiscount { get; set; }
        public decimal TRSED_TaxValue { get; set; }
        public decimal TRSED_TotalAmount { get; set; }
        public bool TRSED_ActiveFlag { get; set; }
        public itemsdetails[] allotteditems { get; set; }
        public DateTime TRSEP_PaymentDate { get; set; }
        public decimal TRSEP_Amount { get; set; }

        public long TRSEP_Id { get; set; }
       
        public string TRSEP_ModeOfPayment { get; set; }
        public string TRSEP_TransactionRefNo { get; set; }
        public string TRSEP_ChequeDDNo { get; set; }
        public DateTime? TRSEP_ChequeDDDate { get; set; }
        
        public string TRSEP_BankName { get; set; }
        public bool TRSEP_ActiveFlag { get; set; }
        public long TRSEP_CreatedBy { get; set; }
        public long TRSEP_UpdatedBy { get; set; }
        public DateTime TRKMLB_FromDate { get; set; }
        public DateTime TRKMLB_ToDate { get; set; }
    }
    public class itemsdetails
    {
        public string TRSED_ItemsName { get; set; }
        public decimal TRSED_Qty { get; set; }
        public string TRSED_Remarks { get; set; }
        public string TRSED_ProblemsListed { get; set; }
        public string TRSED_ServiceDetails { get; set; }
        public decimal TRSED_Amount { get; set; }
        public decimal TRSED_TotalDiscount { get; set; }
        public decimal TRSED_TaxValue { get; set; }
        public decimal TRSED_TotalAmount { get; set; }
    }

}
