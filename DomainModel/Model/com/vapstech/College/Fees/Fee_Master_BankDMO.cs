using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.College.Fees
{
    [Table("Fee_Master_Bank")]
    public class Fee_Master_BankDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FMBANK_Id { get; set; }
        public long MI_Id { get; set; }
        public string FMBANK_BankName { get; set; }
        public string FMBANK_BankDescription { get; set; }
        public bool FMBANK_ActiveFlg { get; set; }
        public DateTime FMBANK_CreatedDate { get; set; }
        public DateTime FMBANK_UpdatedDate { get; set; }
        public long FMBANK_CreatedBy { get; set; }
        public long FMBANK_UpdatedBy { get; set; }
      
    }
}
