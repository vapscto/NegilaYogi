using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.TT
{
    [Table("TT_Master_Building")]
    public class TT_Master_BuildingDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TTMB_Id { get; set; }
        public long MI_Id { get; set; }
        public string TTMB_BuildingName { get; set; }
        public bool TTMB_ActiveFlag { get; set; }
    }
}
