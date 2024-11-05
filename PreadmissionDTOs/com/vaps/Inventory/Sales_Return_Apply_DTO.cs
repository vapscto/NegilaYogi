using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Inventory
{
   public class Sales_Return_Apply_DTO
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long INVMSLRET_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long INVMSL_Id { get; set; }
        public long INVMST_Id { get; set; }
        public decimal INVMSLRETAPP_TotalReturnAmount { get; set; }
        public string INVMSLRETAPP_ReturnRemarks { get; set; }
        public string INVMSLRETAPP_SalesReturnNo { get; set; }
        public bool returnval { get; set; }
        public Array sales_m_return { get; set; }
        public Array sales_item_return { get; set; }

        public arrayretsales1[] arrayretsales { get; set; }

        public class arrayretsales1
        {
            public long INVMI_Id { get; set; }
            public long INVMUOM_Id { get; set; }
            public long INVMP_Id { get; set; }
            public long INVTPIAPP_ApprovedQty { get; set; }
            public decimal invtpI_ApproxAmount { get; set; }
            public string INVTGRNRETAPP_ReturnNaration { get; set; }
            public string flag { get; set; }
        }
    }
}
