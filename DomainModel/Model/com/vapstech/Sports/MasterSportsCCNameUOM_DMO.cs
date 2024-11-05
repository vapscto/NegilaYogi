using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Sports
{
    [Table("SPCC_Master_SportsCCName_UOM",Schema ="SPC")]
    public class MasterSportsCCNameUOM_DMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SPCCMSCCUOM_Id { get; set; }
        public long SPCCMSCC_Id { get; set; }
        public long MI_Id { get; set; }
        public long SPCCMUOM_Id { get; set; }
        public bool SPCCMSCCUOM_ActiveFlag { get; set; }
    }
}
