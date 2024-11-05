using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Sports
{
    [Table("SPCC_Master_Events", Schema ="SPC")]
    public class MasterEventsDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SPCCME_Id { get; set; }
        public long MI_Id { get; set; }
        public string SPCCME_EventName { get; set; }
        public string SPCCME_EventNameDesc { get; set; }
        //public string SPCCME_Flag { get; set; }
        public bool SPCCME_ActiveFlag { get; set; }
    }
}
