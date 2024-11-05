using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Canteen
{
    [Table("CM_Master_FoodItem")]
   public class FooditeamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long CMMFI_Id { get; set; }
    
        public string CMMFI_FoodItemName { get; set; }
        public string CMMFI_FoodItemDescription { get; set; }
        public decimal CMMFI_UnitRate { get; set; }
        public bool CMMFI_OutofStockFlg { get; set; }
        public bool CMMFI_ActiveFlg { get; set; }
        public long CMMFI_CreatedBy { get; set; }
        public long CMMFI_UpdatedBy { get; set; }
        public DateTime? CMMFI_CreatedDate { get; set; }
        public DateTime? CMMFI_Updateddate { get; set; }
        public long? CMMCA_Id { get; set; }
        public bool CMMFI_FoodItemFlag { get; set; }

        public string CMMFI_PathURL { get; set; }
        public List<FooditemimageDMO> FooditemimageDMO { get; set; }


    }
}
