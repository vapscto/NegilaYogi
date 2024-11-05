using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Fee.Tally
{
    [Table("Tally_T_Transaction")]
    class TallyTTransactionDMO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TTT_Id { get; set; }
        public long TMT_Id { get; set; }
        public long TTT_LedgerCode { get; set; }
        public string TTT_LedgerUnder { get; set; }
        public bool TTT_DRCRFlg { get; set; }
        public decimal TTT_Amount { get; set; }
        public string TTT_Naration { get; set; }
        public bool TTT_ActiveFlg { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
