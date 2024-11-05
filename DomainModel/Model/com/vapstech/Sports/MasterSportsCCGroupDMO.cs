using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Sports
{
    [Table("SPCC_Master_SportsCCGroupName", Schema ="SPC")]
    public class MasterSportsCCGroupDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SPCCMSCCG_Id { get; set; }
        public long MI_Id { get; set; }
        public string SPCCMSCCG_SportsCCGroupName { get; set; }
        public string SPCCMSCCG_SportsCCGroupDesc { get; set; }
        public string SPCCMSCCG_SCCFlag { get; set; }
        public bool SPCCMSCCG_ActiveFlag { get; set; }
        public long? SPCCMSCCG_Under { get; set; }
        public long? SPCCMSCCG_Level { get; set; }
    }
}
