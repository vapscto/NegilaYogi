using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Canteen
{
    [Table("CM_Master_FoodItemTax")]
    public  class FooditemtaxDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long CMMFIT_Id { get; set; }
        public long CMMFI_Id { get; set; }
        public long INVMT_Id { get; set; }
        public decimal CMMFIT_TaxPercent { get; set; }
        public bool CMMFIT_ActiveFlg { get; set; }
        public long? CMMFIT_CreatedBy { get; set; }
        public long? CMMFIT_UpdatedBy { get; set; }
        public DateTime? CMMFIT_CreatedDate { get; set; }
        public DateTime? CMMFIT_Updateddate { get; set; }



    }
}
