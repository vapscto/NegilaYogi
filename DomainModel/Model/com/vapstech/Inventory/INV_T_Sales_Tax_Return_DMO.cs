using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("INV_T_Sales_Tax_Return", Schema = "INV")]
    public class INV_T_Sales_Tax_Return_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVTSLTRET_Id { get; set; }
        public long INVTSLRET_Id { get; set; }
        public long INVMT_Id { get; set; }
        public decimal INVTSLTRET_TaxPer { get; set; }
        public decimal INVTSLTRET_TaxAmt { get; set; }
        public bool INVTSLTRET_ActiveFlg { get; set; }
        public DateTime INVTSLTRET_CreatedDate { get; set; }
        public DateTime INVTSLTRET_UpdatedDate { get; set; }
        public long INVTSLTRET_CreatedBy { get; set; }
        public long INVTSLTRET_UpdatedBy { get; set; }
    }
}
