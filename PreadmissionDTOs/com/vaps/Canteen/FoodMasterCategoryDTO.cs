using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Canteen
{
   public class FoodMasterCategoryDTO
    {
        public long MI_Id { get; set; }
        public long CMMCA_Id { get; set; }
        public string CMMCA_CategoryName { get; set; }
        public string CMMCA_Remarks { get; set; }
        public bool CMMCA_ActiveFlag { get; set; }
        public DateTime CMMCA_CreatedDate { get; set; }
        public DateTime CMMCA_UpdatedDate { get; set; }
        public long CreatedBy { get; set; }
        public long UserId { get; set; }
        public long UpdatedBy { get; set; }
        public Array Foodcategeory { get; set; }
        public Array GridviewDetails { get; set; }
        public string returnval { get; set; }




    }
}
