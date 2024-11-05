using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.AssetTracking
{
    public class DisposeAssetsDTO
    {
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public bool already_cnt { get; set; }
        public string message { get; set; }
        public long INVADI_Id { get; set; }
        public long MI_Id { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
        public long INVMST_Id { get; set; }
        public long INVMI_Id { get; set; }
        public long INVMLO_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public DateTime INVADI_DisposedDate { get; set; }
        public decimal INVADI_DisposedQty { get; set; }
        public string INVADI_DisposedRemarks { get; set; }
        public bool INVADI_ActiveFlg { get; set; }
        public string INVMLO_LocationRoomName { get; set; }
        public string INVMLO_LocationRemarks { get; set; }             
        public string INVMLO_InchargeName { get; set; }
        public string INVMS_StoreName { get; set; }
        public string INVMI_ItemName { get; set; }  
        public long INVACO_Id { get; set; }
        public DateTime INVACO_CheckoutDate { get; set; }
        public decimal INVACO_CheckOutQty { get; set; }
        public decimal? INVSTO_SalesRate { get; set; }
        public string INVC_LIFOFIFOFlg { get; set; }
        public long IMFY_Id { get; set; }
        public DateTime? IMFY_FromDate { get; set; }
        public DateTime? IMFY_ToDate { get; set; }
        public string IMFY_FinancialYear { get; set; }
        public string IMFY_AssessmentYear { get; set; }
        public long IMFY_OrderBy { get; set; }
        public Array academicyearlist { get; set; }
        public Array get_store { get; set; }
        public Array get_items { get; set; }
        public Array get_locations { get; set; }
        public Array get_dispose { get; set; }
        public Array get_details { get; set; }
        public string selectionflag { get; set; }
        public Array get_disposereport { get; set; }
        public Array get_Financialyear { get; set; }

        public DisposeitemarrayDTO[] itemarray { get; set; }
        public DisposelocationarrayDTO[] locationarray { get; set; }
    }

    public class DisposeitemarrayDTO
    {
        public long INVMI_Id { get; set; }
        public string INVMI_ItemName { get; set; }
    }
    public class DisposelocationarrayDTO
    {
        public long INVMLO_Id { get; set; }
        public string INVMLO_LocationRoomName { get; set; }
    }
}
