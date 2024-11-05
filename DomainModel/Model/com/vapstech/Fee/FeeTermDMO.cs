using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Fee
{
    [Table("Fee_Master_Terms")]
    public class FeeTermDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FMT_Id { get; set; }
        public long MI_Id { get; set; }
        public string FMT_Name { get; set; }
        public bool FMT_ActiveFlag { get; set; }
        public string FromMonth { get; set; }
        public string ToMonth { get; set; }
        public int? FMT_Order { get; set; }
        public string FMT_Year { get; set; }
        public string Transport_FromMonth { get; set; }
        public string Transport_ToMonth { get; set; }
        public bool? FMT_IncludeArrearFeeFlg { get; set; }
    }
}