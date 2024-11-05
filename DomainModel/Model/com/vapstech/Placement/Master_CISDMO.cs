using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Placement
{
    [Table("PL_CampusInterview_Schedule")]
    public class PL_CampusInterview_ScheduleDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PLCISCH_Id { get; set; }
        public long MI_Id { get; set; }
        public DateTime? PLCISCH_DriveFromDate { get; set; }
        public DateTime? PLCISCH_DriveToDate { get; set; }
        public string PLCISCH_JobDetails { get; set; }
        public bool PLCISCH_OnCampusFlg { get; set; }
        public string PLCISCH_InterviewVenue { get; set; }
        public bool PLCISCH_ActiveFlag { get; set; }
        public DateTime? PLCISCH_CreatedDate { get; set; }
        public DateTime? PLCISCH_UpdatedDate { get; set; }
        public  long PLCISCH_CreatedBy { get; set; }
        public  long PLCISCH_UpdatedBy { get; set; }
    }
}
