using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("INV_T_Sales_Return_Apply", Schema = "INV")]
    public class INV_T_Sales_Return_Apply_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVTSLRETAPP_Id { get; set; }
        public long INVMSLRETAPP_Id { get; set; }
        public long INVMI_Id { get; set; }
        public long INVMUOM_Id { get; set; }
        public long INVMP_Id { get; set; }
        public string INVTSLRETAPP_BatchNo { get; set; }
        public decimal INVTSLRETAPP_SalesReturnQty { get; set; }
        public decimal INVTSLRETAPP_SalesReturnAmount { get; set; }
        public string INVTSLRETAPP_SalesReturnNaration { get; set; }
        public DateTime INVTSLRETAPP_ReturnDate { get; set; }
        public string INVTSLRETAPP_ReturnNo { get; set; }
        public string INVTSLRETAPP_ApproveFlg { get; set; }
        public string INVTSLRETAPP_ReturnNaration { get; set; }
        public bool INVTSLRETAPP_ActiveFlg { get; set; }
        public DateTime? INVTSLRETAPP_CreatedDate { get; set; }
        public DateTime? INVTSLRETAPP_UpdatedDate { get; set; }
        public long INVTSLRETAPP_CreatedBy { get; set; }
        public long INVTSLRETAPP_UpdatedBy { get; set; }
    }
}
