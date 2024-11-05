using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.Sales
{
   public class ISM_Sales_Master_Category_DTO
    {
        public long ISMSMCA_Id { get; set; }
        public long MI_Id { get; set; }
        public string ISMSMCA_CategoryName { get; set; }
        public string ISMSMCA_Remarks { get; set; }
        public bool ISMSMCA_ActiveFlag { get; set; }
        public DateTime? ISMSMCA_CreatedDate { get; set; }
        public DateTime? ISMSMCA_UpdatedDate { get; set; }
        public long CreatedBy { get; set; }
        public long UpdatedBy { get; set; }
        public Array category_list { get; set; }
        public Array edit_category_list { get; set; }
        public string returnvalue { get; set; }
        public long userId { get; set; }
        public bool retbool { get; set; }

    }
}
