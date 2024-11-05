using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Transport
{
    [Table("TR_Service", Schema = "TRN")]
    public class TR_ServiceDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TRSE_Id { get; set; }
        public long  MI_Id { get; set; }
        public long TRMSST_Id { get; set; }
        public string TRSE_ServiceStationName { get; set; }
        public long TRMV_Id { get; set; }
        public long TRMD_Id { get; set; }
        public string TRSE_ServiceRefNo { get; set; }
        public string TRSE_ProblemsListed { get; set; }
        public string TRSE_ServiceDetails { get; set; }
        public string TRSE_BillNo { get; set; }
        public decimal TRSE_LabourCharges { get; set; }
        public decimal TRSE_ItemsCost { get; set; }
        public DateTime? TRSE_BillDate   { get; set; }
        public DateTime TRSE_ServiceDate { get; set; }
        public decimal TRSE_TotalBillValue { get; set; }
        public decimal TRSE_TotalDiscount { get; set; }
        public decimal  TRSE_TaxValue { get; set; }
        public decimal  TRSE_TDSValue { get; set; }
        public decimal TRSE_TotalPaid { get; set; }
        public bool TRSE_ActiveFlag { get; set; }
        public long TRSE_CreatedBy { get; set; }
        public long TRSE_UpdatedBy { get; set; }
       public List<TR_Service_DetailsDMO> TR_Service_DetailsDMO { get; set; } 
       
    }
}
