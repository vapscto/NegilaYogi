using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("INV_M_Sales_Return_Apply", Schema = "INV")]
    public class INV_M_Sales_Return_Apply_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVMSLRETAPP_Id { get; set; }
        public long MI_Id { get; set; }
        public long INVMST_Id { get; set; }
        public long INVMSL_Id { get; set; }
        public string INVMSLRETAPP_SalesReturnNo { get; set; }
        public DateTime INVMSLRETAPP_SalesReturnDate { get; set; }
        public decimal INVMSLRETAPP_TotalReturnAmount { get; set; }
        public string INVMSLRETAPP_ReturnRemarks { get; set; }
        public string INVMSLRETAPP_StatusFlg { get; set; }
        public bool INVMSLRETAPP_ActiveFlg { get; set; }
        public DateTime? INVMSLRETAPP_CreatedDate { get; set; }
        public DateTime? INVMSLRETAPP_UpdatedDate { get; set; }
        public long INVMSLRETAPP_CreatedBy { get; set; }
        public long INVMSLRETAPP_UpdatedBy { get; set; }
        public List<INV_T_Sales_Return_Apply_DMO> INV_T_Sales_Return_Apply_DMO { get; set; }
        public List<INV_M_Sales_Return_DMO> INV_M_Sales_Return_DMO { get; set; }
    }
}
