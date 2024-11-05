using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Transport
{
    [Table("TR_DistanceChart", Schema = "TRN")]
    public class DriverChartDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TRDC_Id { get; set; }
        public long MI_Id { get; set; }
        public decimal TRDC_FromKM { get; set; }
        public decimal TRDC_ToKM { get; set; }
        public decimal TRDC_RateKm { get; set; }
        public DateTime? TRDC_Date { get; set; }
        public long TRMV_Id { get; set; }
        public long TRMD_Id { get; set; }
        public string TRDC_Indent_BillNo { get; set; }
        public decimal TRDC_NoofLtr { get; set; }
        public decimal TRDC_TotalKM { get; set; }
        public decimal TRDC_TotalMileage { get; set; }
        public decimal TRDC_TotalAmount { get; set; }
        public decimal TRDC_Emission { get; set; }
        public decimal TRDC_AddtAmt { get; set; }
        public string TRDC_Remarks { get; set; }
        public decimal TRDC_GrossAmount { get; set; }

    }
}
