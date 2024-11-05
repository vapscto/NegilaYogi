using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DomainModel.Model.com.vapstech.TT
{
    [Table("TT_Master_Period")]
    public class TT_Master_PeriodDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int TTMP_Id { get; set; }
        public long MI_Id { get; set; }
        public string TTMP_PeriodName { get; set; }
        public bool TTMP_ActiveFlag { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
