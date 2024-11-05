using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Sports
{
    [Table("SPCC_Events_Team_Students", Schema = "SPC")]
    public class EventsTeamStudentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long SPCCETMSTD_Id { get; set; }
        public long SPCCETM_Id { get; set; }
        public long AMST_Id { get; set; }
        public bool SPCCETMSTD_ActiveFlag { get; set; }
        public DateTime? SPCCETMSTD_CreatedDate { get; set; }
        public DateTime? SPCCETMSTD_UpdatedDate { get; set; }
        public long SPCCETMSTD_CreatedBy { get; set; }
        public long SPCCETMSTD_UpdatedBy { get; set; }

    }
}
