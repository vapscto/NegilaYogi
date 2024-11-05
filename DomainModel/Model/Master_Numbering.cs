using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("IVRM_Master_Numbering")]
    public class Master_Numbering : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IMN_Id { get; set; }
        public long MI_Id { get; set; }
        public string IMN_AutoManualFlag { get; set; }
        public string IMN_DuplicatesFlag { get; set; }
        public string IMN_StartingNo { get; set; }
        public string IMN_WidthNumeric { get; set; }
        public string IMN_ZeroPrefixFlag { get; set; }
        public bool IMN_PrefixAcadYearCode { get; set; }
        public string IMN_PrefixParticular { get; set; }
        public bool IMN_SuffixAcadYearCode { get; set; }
        public string IMN_SuffixParticular { get; set; }
        public string IMN_RestartNumFlag { get; set; }
        public string IMN_Flag { get; set; }

        //
        public bool? IMN_PrefixFinYearCode { get; set; }
        public bool? IMN_PrefixCalYearCode { get; set; }

        public bool? IMN_SuffixFinYearCode { get; set; }
        public bool? IMN_SuffixCalYearCode { get; set; }

    }
}
