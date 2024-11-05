using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModel.Model.com.vaps.Fee;

namespace DomainModel.Model.com.vapstech.College.Fees
{
    [Table("Fee_College_T_Fine_Slabs", Schema = "CLG")]
    public class CLG_Fee_College_T_Fine_Slabs
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FCTFS_Id { get; set; }
        public long FMFS_Id { get; set; }
        public long FCMAS_Id { get; set; }
        public string FCTFS_FineType { get; set; }
        public decimal FCTFS_Amount { get; set; }

        public string FCTFS_PercentageFlg { get; set; }


    }
}
