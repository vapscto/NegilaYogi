using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Canteen
{
    [Table("CM_Master_Category")]
    public  class FoodMasterCategoryDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CMMCA_Id { get; set; }
        public long MI_Id { get; set; }
     
        public string CMMCA_CategoryName { get; set; }
        public string CMMCA_Remarks { get; set; }
        public bool CMMCA_ActiveFlag { get; set; }
        public DateTime CMMCA_CreatedDate { get; set; }
        public DateTime CMMCA_UpdatedDate { get; set; }
        public long CreatedBy { get; set; }
        public long UpdatedBy { get; set; }
 
    }
}
