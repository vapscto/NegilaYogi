using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("INV_M_Sales_Return", Schema = "INV")]
    public class INV_M_Sales_Return_DMO
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVMSLRET_Id { get; set; }
        public long MI_Id { get; set; }
        public long INVMST_Id { get; set; }
        public long INVMSL_Id { get; set; }
        public string INVMSLRET_SalesReturnNo { get; set; }
        public DateTime INVMSLRET_SalesReturnDate { get; set; }
        public DateTime? INVMSLRET_CreditNoteDate { get; set; }
        public decimal INVMSLRET_TotalReturnAmount { get; set; }
        public string INVMSLRET_ReturnRemarks { get; set; }
        public string INVMSLRET_CreditNoteNo { get; set; }
        public string INVMSLRET_EWayRefNo { get; set; }
        public bool INVMSLRET_ActiveFlg { get; set; }
        public DateTime INVMSLRET_CreatedDate { get; set; }
        public DateTime INVMSLRET_UpdatedDate { get; set; }
        public long INVMSLRET_CreatedBy { get; set; }
        public long INVMSLRET_UpdatedBy { get; set; }
        public long INVMSLRETAPP_Id { get; set; }
        public List<INV_T_Sales_Return_DMO> INV_T_Sales_Return_DMO { get; set; }
    }
}
