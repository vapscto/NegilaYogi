using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Fee
{
    [Table("Fee_T_Fine_Slabs_OthStaffs")]
    public class Fee_T_Fine_Slabs_OthStaffs :CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long FTFSOST_Id { get; set; }
        public long FMFS_Id { get; set; }
        public long FMAOST_Id { get; set; }
        public string FTFSOST_FineType { get; set; }
        public decimal FTFSOST_Amount { get; set; }
        
    }
}


