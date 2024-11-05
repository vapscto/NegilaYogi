using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.com.vapstech.TT
{
    [Table("TT_Master_Staff_Abbreviation")]
    public class TT_Master_Staff_AbbreviationDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TTMSAB_Id { get; set; }      
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public string TTMSAB_Abbreviation { get; set; }      
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool TTMSAB_ActiveFlag { get; set; }

        public long? TTMSAB_PerDayMaxDeputation { get; set; }
        public long? TTMSAB_PerWeekMaxDeputation { get; set; }
        public long? TTMSAB_PerMonthMaxDeputation { get; set; }
        public long? TTMSAB_PerYearMaxDeputation { get; set; }

    }
}
