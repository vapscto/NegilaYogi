using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Sports
{
    [Table("SPCC_Events_Sponsor",Schema ="SPC")]
    public class EventsSponsorDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SPCCESP_Id { get; set; }
        public long MI_Id { get; set; }
        public long SPCCE_Id { get; set; }
        public long SPCCMSP_Id { get; set; }
        public bool SPCCESP_ActiveFlag { get; set; }


    }
}
