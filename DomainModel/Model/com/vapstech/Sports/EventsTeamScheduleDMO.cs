using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text;

namespace DomainModel.Model.com.vapstech.Sports
{
    [Table("SPCC_Events_Team_Schedule", Schema = "SPC")]
    public class EventsTeamScheduleDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SPCCETMSCH_Id { get; set; }
        public long MI_Id { get; set; }
        public string SPCCETMSCH_Team1 { get; set; }
        public string SPCCETMSCH_Team2 { get; set; }
        public DateTime SPCCETMSCH_Date { get; set; }
        public string SPCCETMSCH_Time { get; set; }
        public string SPCCETMSCH_Result { get; set; }
        public string SPCCETMSCH_Remarks { get; set; }
        public bool SPCCETMSCH_ActiveFlag { get; set; }
        public DateTime SPCCETMSCH_CreatedDate { get; set; }
        public DateTime SPCCETMSCH_UpdatedDate { get; set; }
        public long SPCCETMSCH_CreatedBy { get; set; }
        public long SPCCETMSCH_UpdatedBy { get; set; }
    }
}
