using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Sports
{
    public class SportStudentParticipationReportDTO : CommonParamDTO
    {
        public long SPCCMD_Id { get; set; }
        public long SPCCMCC_Id { get; set; }
        public long SPCCMEV_Id { get; set; }
        public long MI_Id { get; set; }
        public string SPCCMEV_EventVenue { get; set; }
        public string SPCCMEV_EventVenueDesc { get; set; }
        public bool SPCCMEV_ActiveFlag { get; set; }
        public long AMST_Id { get; set; }
        public string studentname { get; set; }
        public Array yearlist { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long SPCCME_Id { get; set; }
        public long SPCCMSCC_Id { get; set; }
        public long SPCCMH_Id { get; set; }
        public Array ClassList { get; set; }
        public Array SectionList { get; set; }

        public string Type { get; set; }
        public SportStudentParticipationReportDTO[] StudentList { get; set; }
        public Array StudentList1 { get; set; }
        public Array DivisionList { get; set; }
        public Array venueList { get; set; }
        public Array eventList { get; set; }
        public Array houseList { get; set; }

        public SportStudentParticipationReportDTO[] selectedhouselist { get; set; }
        public SportStudentParticipationReportDTO[] selectedSectionlist { get; set; }
        public SportStudentParticipationReportDTO[] selectedeventlist { get; set; }
        public SportStudentParticipationReportDTO[] selectedsportslist { get; set; }
        public SportStudentParticipationReportDTO[] selectedVenuelist { get; set; }
        public SportStudentParticipationReportDTO[] placListData { get; set; }
        public Array viewlist { get; set; }
        public string classname { get; set; }
        public string sectionname { get; set; }
        public DateTime? SPCCE_StartDate { get; set; }
        public string SPCCME_EventName { get; set; }
        public string SPCCMH_HouseName { get; set; }
        public string SPCCMD_DivisionName { get; set; }
        public string adm_no { get; set; }
        public string AMST_MotherName { get; set; }
        public string AMST_FatherName { get; set; }
        public string Address { get; set; }
        public int? PASR_Age { get; set; }
        public DateTime AMST_DOB { get; set; }
        public Array sportslist { get; set; }
        public Array categoryList { get; set; }

        public int SPCCSHD_Age { get; set; }
        public decimal SPCCSHD_Height { get; set; }
        public decimal SPCCSHD_Weight { get; set; }
        public class Ostudent
        {
            public long  AMST_Id { get; set; }
            public string studentname { get; set; }
        }
        public bool returnVal { get; set; }
        public string radiotype { get; set; }
        public Array eventname { get; set; }
        public long SPCCPM_Id { get; set; }
        public Array events { get; set; }
        public string SPCCPM_Name { get; set; }
        public long SPCCE_Id { get; set; }
        public int SPCCESTR_Place { get; set; }
        public double SPCCESTR_Points { get; set; }
        public string SPCCESTR_Rank { get; set; }
        public Array viewlistCCwise { get; set; }
    }
}
