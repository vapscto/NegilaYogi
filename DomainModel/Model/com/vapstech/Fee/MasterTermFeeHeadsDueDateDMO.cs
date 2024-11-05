using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Fee
{
    [Table("Fee_Master_Terms_FeeHeads_DueDate")]
    public class MasterTermFeeHeadsDueDateDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FMTFHDD_Id { get; set; }
        public long FMTFH_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public DateTime FMTFHDD_FromDate { get; set; }
        public DateTime FMTFHDD_ToDate { get; set; }
        public DateTime FMTFHDD_ApplicableDate { get; set; }
        public DateTime FMTFHDD_DueDate { get; set; }
        public long MI_Id { get; set; }
    }
}
