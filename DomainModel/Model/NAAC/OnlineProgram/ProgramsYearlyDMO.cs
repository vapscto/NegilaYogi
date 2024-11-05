using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.OnlineProgram
{
    [Table("Programs_Yearly")]
    public class ProgramsYearlyDMO :CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PRYR_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string PRYR_ProgramName { get; set; }
        public DateTime PRYR_StartDate { get; set; }
        public DateTime? PRYR_EndDate { get; set; }
        public string PRYR_StartTime { get; set; }
        public string PRYR_EndTime { get; set; }
        public string PRYR_ProgramDescription { get; set; }
        public bool PRYR_ActiveFlag { get; set; }
        public long PRYR_CreatedBy { get; set; }
        public long PRYR_UpdatedBy { get; set; }
        public string PRYR_ProgramChart { get; set; }
        public string PRYR_ProgramChartPath { get; set; }
        public long? PRYR_PrgramLevel { get; set; }
        public long? PRYR_ProgramTypeId { get; set; }
        public string PRYR_PrgramConvenor { get; set; }
        public long? PRYR_TotalParticipants { get; set; }
        public string PRYR_ProgramInvitation { get; set; }
        public string PRYR_ParticipantList { get; set; }
        public string PRYR_PListPath { get; set; }
        public string PRYR_AccountStatement { get; set; }
        public string PRYR_ASPath { get; set; }
        public string PRYR_WinnerList { get; set; }
        public string PRYR_WListPath { get; set; }
        public string PRYR_SponsorAgency { get; set; }
        public long? PRYR_IntParticipants { get; set; }
        public long? PRYR_OthCollStudents { get; set; }
        public long? PRYR_NatParticipants { get; set; }
        public long? PRYR_ResearchScholars { get; set; }
        public long? PRYR_OurCollStudents { get; set; }
        public bool PRYR_LecturesFlg { get; set; }
        public long PRYR_LecturesNo { get; set; }
        public bool PRYR_TrainingFlg { get; set; }
        public long PRYR_TrainingNo { get; set; }
        public bool? PRYR_OralPresentationFlg { get; set; }
        public long? PRYR_OralPresentation { get; set; }
        public bool PRYR_PosterPresentationFlg { get; set; }
        public long? PRYR_PosterPresentation { get; set; }
        public long? PRYR_Faculty { get; set; }
        public long? HRMD_Id { get; set; }
        public List<ProgramsYearlyFileDMO> ProgramsYearlyFileDMO { get; set; }
        public List<ProgramsYearlyGuestDMO> ProgramsYearlyGuestDMO { get; set; }
        public List<ProgramsYearlyActivitiesDMO> ProgramsYearlyActivitiesDMO { get; set; }
    }
}
