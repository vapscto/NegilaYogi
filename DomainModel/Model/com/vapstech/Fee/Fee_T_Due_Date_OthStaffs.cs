using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Fee
{
    [Table("Fee_T_Due_Date_OthStaffs")]

    public class Fee_T_Due_Date_OthStaffs:CommonParamDMO

    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long FTDDOST_Id { get; set; }
        public long FMAOST_Id { get; set; }
        public string FTDD_Day { get; set; }
        public string FTDD_Month { get; set; }

        public int FTDD_Year { get; set; }

    }
}
