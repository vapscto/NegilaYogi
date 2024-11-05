using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Fee
{
    [Table("IVRM_Master_PG")]
    public class PAYUDETAILS:CommonParamDMO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IMPG_Id { get; set; }
        public string IMPG_PGName { get; set; }
        public string IMPG_PGFlag { get; set; }
        public bool IMPG_ActiveFlg { get; set; }


        public string IMPG_IndustryType { get; set; }
        public string IMPG_Website { get; set; }
        public string IMPG_TransactionStatusURL { get; set; }
        public string IMPG_SettlementURL { get; set; }

    }
}
