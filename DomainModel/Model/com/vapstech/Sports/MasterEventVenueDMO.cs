using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Sports
{
    [Table("SPCC_Master_EventVenue", Schema ="SPC")]
    public class MasterEventVenueDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SPCCMEV_Id { get; set; }
        public long MI_Id { get; set; }
        public string SPCCMEV_EventVenue { get; set; }
        public string SPCCMEV_EventVenueDesc { get; set; }
        public bool SPCCMEV_ActiveFlag { get; set; }
    }
}
