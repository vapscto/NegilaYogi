﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("IVRM_Voucher_Numbering")]
    public class Voucher_Numbering :CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IVN_Id { get; set; }
        public long MI_Id { get; set; }
        public string IVN_VoucherName { get; set; }
        public string IVN_VoucherID { get; set; }
        public string IVN_AutoManualFlag { get; set; }
        public string IVN_DuplicatesFlag { get; set; }
        public string IVN_StartingNo { get; set; }
        public string IVN_WidthNumeric { get; set; }
        public string IVN_ZeroPrefixFlag { get; set; }
        public bool? IVN_PrefixFinYearCode { get; set; }
        public string IVN_PrefixParticular { get; set; }
        public bool? IVN_SuffixFinYearCode { get; set; }
        public string IVN_SuffixParticular { get; set; }
        public string IVN_RestartNumFlag { get; set; }
    }
}