using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Sports
{
    [Table("SPCC_Events_Team", Schema = "SPC")]
    public class EventsTeamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long SPCCETM_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long SPCCE_Id { get; set; }
        public long SPCCMCL_Id { get; set; }
        public long SPCCMCC_Id { get; set; }
        public long SPCCMSCC_Id { get; set; }
        public string SPCCETM_TeamName { get; set; }
        public long SPCCETM_NoOfParticipants { get; set; }
        public bool SPCCETM_ActiveFlag { get; set; }
        public DateTime SPCCETM_CreatedDate { get; set; }
        public DateTime SPCCETM_UpdatedDate { get; set; }
        public List<EventsTeamStudentsDMO> EventsTeamStudentsDMO { get; set; }
        
    }
}
