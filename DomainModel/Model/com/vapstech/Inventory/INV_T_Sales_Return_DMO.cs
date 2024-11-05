using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("INV_T_Sales_Return", Schema = "INV")]
    public class INV_T_Sales_Return_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVTSLRET_Id { get; set; }
        public long INVMSLRET_Id { get; set; }
        public long INVMI_Id { get; set; }
        public long INVMUOM_Id { get; set; }
        public long INVMP_Id { get; set; }
        public string INVTSLRET_BatchNo { get; set; }
        public decimal INVTSLRET_SalesReturnQty { get; set; }
        public decimal INVTSLRET_SalesReturnAmount { get; set; }
        public string INVTSLRET_SalesReturnNaration { get; set; }
        public DateTime INVTSLRET_ReturnDate { get; set; }
        public string INVTSLRET_ReturnNo { get; set; }
        public string INVTSLRET_ReturnNaration { get; set; }
        public bool INVTSLRET_ActiveFlg { get; set; }
        public DateTime INVTSLRET_CreatedDate { get; set; }
        public DateTime INVTSLRET_UpdatedDate { get; set; }
        public long INVTSLRET_CreatedBy { get; set; }
        public long INVTSLRET_UpdatedBy { get; set; }
        public List<INV_T_Sales_Tax_Return_DMO> INV_T_Sales_Tax_Return_DMO { get; set; }
    }
}
