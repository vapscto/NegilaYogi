using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.Sales
{
   public  class ISM_Sales_Master_Product_DTO
    {
        public long ISMSMPR_Id { get; set; }
        public long MI_Id { get; set; }
        public string ISMSMPR_ProductName { get; set; }
        public string ISMSMPR_Remarks { get; set; }
        public bool ISMSMPR_ActiveFlag { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long ISMSMPR_CreatedBy { get; set; }
        public long ISMSMPR_UpdatedBy { get; set; }
        public string returnvales { get; set; }
        public bool retbool { get; set; }
        public long userId { get; set; }
        public Array product_list { get; set; }
        public Array edit_product_list { get; set; }
    }
}
