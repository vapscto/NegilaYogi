using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.com.vapstech.TT
{
    [Table("TT_Final_Generation")]
    public class TT_Final_GenerationDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TTFG_Id { get; set; }
        public long MI_Id { get; set; }
        public long TTMC_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string TTFG_VersionNo { get; set; }
        public bool TTFG_ActiveFlag { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
