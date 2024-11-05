using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Portals.Employee
{
    [Table("LMS_Live_Meeting")]
    public class LMS_Live_MeetingDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long  LMSLMEET_Id { get; set; }
        public long MI_Id { get; set; }
        public string LMSLMEET_MeetingId { get; set; }
        public long User_Id { get; set; }
        public long HRME_Id { get; set; }
        public string LMSLMEET_PIPAddress { get; set; }
        public string LMSLMEET_PMACAddress { get; set; }
        public string LMSLMEET_PlannedStartTime { get; set; }
        public string LMSLMEET_PlannedEndTime { get; set; }
        public string LMSLMEET_MeetingTopic { get; set; }
        public DateTime LMSLMEET_PlannedDate { get; set; }
        public DateTime? LMSLMEET_MeetingDate { get; set; }
        public string LMSLMEET_StartedTime { get; set; }
        public string LMSLMEET_EndTime { get; set; }
        public string LMSLMEET_IPAddress { get; set; }
        public string LMSLMEET_MACAddress { get; set; }
     
        public bool LMSLMEET_ActiveFlg { get; set; }
        public bool LMSLMEET_CanOthersStartFlg { get; set; }
        public DateTime LMSLMEET_CreatedDate { get; set; }
        public DateTime LMSLMEET_UpdatedDate { get; set; }
        public long LMSLMEET_CreatedBy { get; set; }
        public long LMSLMEET_UpdatedBy { get; set; }
        public long? LMSLMEET_StartedByUserId { get; set; }
        public string LMSLMEET_MeetingURL { get; set; }
        public bool LMSLMEET_Recordflag { get; set; }
        public string LMSLMEET_RecordId { get; set; }
        public string LMSLMEET_internalMeetingID { get; set; }
        public string LMSLMEET_Remarks { get; set; }
        public string LMSLMEET_Grade { get; set; } 
        public List<LMS_Live_Meeting_ClassDMO> LMS_Live_Meeting_ClassDMO { get; set; }
        public List<LMS_Live_Meeting_StaffOthersDMO> LMS_Live_Meeting_StaffOthersDMO { get; set; }
        public List<LMS_Live_Meeting_CourseBranchDMO> LMS_Live_Meeting_CourseBranchDMO { get; set; }
        //public List<LMS_Live_Meeting_PAStudentDMO> LMS_Live_Meeting_PAStudentDMO { get; set; }

    }
}
