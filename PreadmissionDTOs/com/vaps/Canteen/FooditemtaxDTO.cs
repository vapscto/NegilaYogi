using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Canteen
{
    public class FooditemtaxDTO
    {
        public long CMMFIT_Id { get; set; }
        public long CMMFI_Id { get; set; }
        public long INVMT_Id { get; set; }
        public long CMMCA_Id { get; set; }
        public decimal CMMFIT_TaxPercent { get; set; }
        public bool CMMFIT_ActiveFlg { get; set; }
        public long CMMFIT_CreatedBy { get; set; }
        public long CMMFIT_UpdatedBy { get; set; }
        public DateTime CMMFIT_CreatedDate { get; set; }
        public long MI_Id { get; set; }
        public Array foodtax { get; set; }
        public Array Fooditeam { get; set; }
        public Array invmaster { get; set; }
        public string returnval { get; set; }
        public long UserId { get; set; }
        public string CMMFI_FoodItemName { get; set; }
        public string CMMCA_CategoryName { get; set; }
        public string INVMT_TaxName { get; set; }
        public decimal taxpercent { get; set; }
        public decimal CMMFI_UnitRate { get; set; }
        public bool CMMFI_OutofStockFlg { get; set; }
        public bool CMMFI_ActiveFlg { get; set; }


    }
}
