using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Transport
{
    [Table("TR_Service_Details", Schema = "TRN")]
    public class TR_Service_DetailsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TRSED_Id { get; set; }
        public long TRSE_Id { get; set; }
        public string TRSED_ItemsName { get; set; }
        public decimal TRSED_Qty { get; set; }
        public string TRSED_Remarks { get; set; }
        public string TRSED_ProblemsListed { get; set; }
        public string TRSED_ServiceDetails { get; set; }
        public decimal TRSED_Amount { get; set; }
        public decimal TRSED_TotalDiscount { get; set; }
        public decimal TRSED_TaxValue { get; set; }
        public decimal TRSED_TotalAmount { get; set; }
        public bool TRSED_ActiveFlag { get; set; }
        public long TRSED_CreatedBy { get; set; }
        public long TRSED_UpdatedBy { get; set; }


    }
}
