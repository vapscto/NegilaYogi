using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Inventory
{
    public class INV_DashboardDTO
    {
        public bool? returnval { get; set; }
        public string message { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long INVSTO_Id { get; set; }
        public long INVMI_Id { get; set; }
        public string INVMI_ItemName { get; set; }
        public decimal? INVSTO_SalesRate { get; set; }
        public decimal? INVSTO_AvaiableStock { get; set; }
             public DateTime? INVSTO_PurchaseDate { get; set; }
        public decimal? totPurchase { get; set; }
        public decimal? totSales { get; set; }
        public decimal? totAvailableStock { get; set; }
        public decimal? totCheckout { get; set; }
        public string typeflg { get; set; }
        public long? expiredays { get; set; }
        public long totItem { get; set; }
        public Array totalowStock { get; set; }      
        public Array totalWexpire { get; set; }
        public Array dashboardgrid { get; set; }
        public Array warrantydetails { get; set; }
        public Array totalWexpired { get; set; }
        public Array totalAvailableStock { get; set; }
    }

}
