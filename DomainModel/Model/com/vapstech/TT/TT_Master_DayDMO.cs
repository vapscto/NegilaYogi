using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.TT
{
    [Table("TT_Master_Day")]
    public class TT_Master_DayDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TTMD_Id { get; set; }
        public long MI_Id { get; set; }
        public string TTMD_DayName { get; set; }
        public string TTMD_DayCode { get; set; }
        public long Order_Id { get; set; }
        public bool TTMD_ActiveFlag { get; set; }
    }
}
