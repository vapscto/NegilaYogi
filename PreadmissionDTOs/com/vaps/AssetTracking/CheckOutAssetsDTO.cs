using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.AssetTracking
{
    public class CheckOutAssetsDTO
    {
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public bool already_cnt { get; set; }
        public string message { get; set; }
        public long INVACO_Id { get; set; }
        public long MI_Id { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
        public long INVMST_Id { get; set; }
        public long INVMI_Id { get; set; }
        public long INVMLO_Id { get; set; }
        public long INVMSI_Id { get; set; }
        public DateTime INVACO_CheckoutDate { get; set; }
        public decimal INVACO_CheckOutQty { get; set; }
        public string INVACO_ReceivedBy { get; set; }
        public string INVACO_CheckOutRemarks { get; set; }
        public bool INVACO_ActiveFlg { get; set; }
        public long? HRME_Id { get; set; }
        public string INVMLO_LocationRoomName { get; set; }
        public string INVMLO_LocationRemarks { get; set; }
        public string INVMLO_InchargeName { get; set; }
        public string INVMS_StoreName { get; set; }
        public string INVMI_ItemName { get; set; }
        public decimal? INVSTO_AvaiableStock { get; set; }
        public string INVC_LIFOFIFOFlg { get; set; }

        public Array academicyearlist { get; set; }
        public Array financial_year { get; set; }
        public DateTime? INVSTO_PurchaseDate { get; set; }
        public decimal? INVSTO_SalesRate { get; set; }
        public string employeename { get; set; }
        public string contactflag { get; set; }
        public string selectionflag { get; set; }

        public long IMFY_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public DateTime? IMFY_FromDate { get; set; }
        public DateTime? IMFY_ToDate { get; set; }
        public string IMFY_FinancialYear { get; set; }
        public string IMFY_AssessmentYear { get; set; }
        public long IMFY_OrderBy { get; set; }

        public Array get_contactperson { get; set; }
        public Array get_store { get; set; }
        public Array get_items { get; set; }
        public Array get_locations { get; set; }
        public Array get_checkout { get; set; }
        public Array get_employee { get; set; }
        public Array availablestock { get; set; }
        public Array get_checkoutreport { get; set; }
        public Array get_AssetsReportdetails { get; set; }
        public Array get_AssetsReport { get; set; }
        public Array get_Financialyear { get; set; }
        public Array get_locationDetails { get; set; }
        public Array employeedropdown { get; set; }

        public itemarrayDTO[] itemarray { get; set; }
        public locationarrayDTO[] locationarray { get; set; }
        public employeearrDTO[] emparray { get; set; }
    }

    public class itemarrayDTO
    {
        public long INVMI_Id { get; set; }
        public string INVMI_ItemName { get; set; }
        
    }
    public class locationarrayDTO
    {
        public long INVMLO_Id { get; set; }
        public string INVMLO_LocationRoomName { get; set; }
    }
    public class employeearrDTO
    {
        public long HRME_Id { get; set; }
    }
}
