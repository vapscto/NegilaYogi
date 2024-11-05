using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.College.Fees
{
    [Table("Fee_College_T_Due_Date", Schema = "CLG")]
    public class CLG_Fee_College_T_Due_DateDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FCTDD_Id { get; set; }
        public long FCMAS_Id { get; set; }
        public string FCTDD_Day { get; set; }
        public string FCTDD_Month { get; set; }
        public string FCTDD_Year { get; set; }
    }
}
