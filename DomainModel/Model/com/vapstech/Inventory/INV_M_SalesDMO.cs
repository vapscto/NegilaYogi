using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("INV_M_Sales", Schema = "INV")]
    public class INV_M_SalesDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVMSL_Id { get; set; }
        public long MI_Id { get; set; }
        public long INVMST_Id { get; set; }
        public string INVMSL_StuOtherFlg { get; set; }
        public string INVMSL_SalesNo { get; set; }
        public DateTime? INVMSL_SalesDate { get; set; }
        public decimal? INVMSL_SalesValue { get; set; }
        public decimal? INVMSL_TotDiscount { get; set; }
        public decimal? INVMSL_TotTaxAmt { get; set; }
        public decimal? INVMSL_TotalAmount { get; set; }
        public string INVMSL_Remarks { get; set; }
        public string INVMSL_ReturnFlg { get; set; }
        public string INVMSL_PaidFlg { get; set; }
        public bool? INVMSL_CreditFlg { get; set; }
        public bool? INVMSL_ActiveFlg { get; set; }
        public long INVMP_Id { get; set; }   

        public List<INV_T_SalesDMO> INV_T_SalesDMO { get; set; }
        public List<INV_M_Sales_CustomerDMO> INV_M_Sales_CustomerDMO { get; set; }
        public List<INV_M_Sales_StaffDMO> INV_M_Sales_StaffDMO { get; set; }
        public List<INV_M_Sales_StudentDMO> INV_M_Sales_StudentDMO { get; set; }





    }
}
