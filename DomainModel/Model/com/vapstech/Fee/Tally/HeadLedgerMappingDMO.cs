using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Fee.Tally
{
    [Table("Fee_Yearly_Group_Head_LedgerMapping")]
    public class HeadLedgerMappingDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FYGHLM_Id { get; set; }
        public long FYGHM_Id { get; set; }
        public string FYGHM_RVRegLedgerId { get; set; }
        public string FYGHM_RVRegLedgerUnder { get; set; }
        public string FYGHM_RVAdvanceLegderId { get; set; }
        public string FYGHM_RVAdvanceLegderUnder { get; set; }
        public string FYGHM_RVArrearLedgerId { get; set; }
        public string FYGHM_RVArrearLedgerUnder { get; set; }
        public string FYGHM_JVRegLedgerId { get; set; }
        public string FYGHM_JVRegLedgerUnder { get; set; }
        public string FYGHM_JVAdvanceLegderId { get; set; }
        public string FYGHM_JVAdvanceLegderUnder { get; set; }
        public string FYGHM_JVArrearLedgerId { get; set; }
        public string FYGHM_JVArrearLedgerUnder { get; set; }
    }
}
