using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Sports
{
    [Table("SPCC_Events", Schema ="SPC")]
    public class EventsMappingDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SPCCE_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long SPCCME_Id { get; set; }
        public long SPCCMEV_Id { get; set; }
        public DateTime? SPCCE_StartDate { get; set; }
        public string SPCCE_StartTime { get; set; }
        public DateTime? SPCCE_EndDate { get; set; }
        public string SPCCE_EndTime { get; set; }
        public string SPCCE_Remarks { get; set; }
        public bool SPCCE_ActiveFlag { get; set; }
        public bool SPCCE_SponsorFlag { get; set; }


        public List<EventsSponsorDMO> EventsSponsorDMO { get; set; }

    }
}
