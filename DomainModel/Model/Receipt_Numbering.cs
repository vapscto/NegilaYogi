using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("IVRM_Receipt_Numbering")]
    public class Receipt_Numbering : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IRN_Id { get; set; }
        public long MI_Id { get; set; }
        public string IRN_TransactionName { get; set; }
        public string IRN_AutoManualFlag { get; set; }
        public string IRN_DuplicatesFlag { get; set; }
        public string IRN_StartingNo { get; set; }
        public string IRN_WidthNumeric { get; set; }
        public string IRN_ZeroPrefixFlag { get; set; }
        public bool IRN_PrefixAcadYearCode { get; set; }
        public bool? IRN_PrefixFinYearCode { get; set; }
        public bool? IRN_PrefixCalYearCode { get; set; }
        public string IRN_PrefixParticular { get; set; }
        public bool IRN_SuffixAcadYearCode { get; set; }
        public bool? IRN_SuffixFinYearCode { get; set; }
        public bool? IRN_SuffixCalYearCode { get; set; }
        public string IRN_SuffixParticular { get; set; }
        public string IRN_RestartNumFlag { get; set; }
        public bool? IRN_RestartAcadYear { get; set; }
        public bool? IRN_RestartFinYear { get; set; }
        public bool? IRN_RestartcalendYear { get; set; }
    }
}
