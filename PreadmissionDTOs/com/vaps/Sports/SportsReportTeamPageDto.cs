using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Sports
{
    public class SportsReportTeamPageDto
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public Array yearlist { get; set; }
        public Array categoryList { get; set; }
        public Array CompetetionLevel { get; set; }
        public Categorylist[] Categorylists { get; set; }
        public CompetetionLeveltemp[] CompetetionLevels { get; set; }
        public Sportleveltemp[] Sportleveltemps { get; set; }
     //   public SportName[] SportNames { get; set; }
        public Array MasterEvent { get; set; }
        public Array sportsName { get; set; }
        public long SPCCME_Id { get; set; }
        public string className { get; set; }
        public Array classDrpDwn { get; set; }
        public School_M_ClassDTO[] selectedClass { get; set; }
        public Array studentList { get; set; }
        public long? ASMCL_Id { get; set; }
        public string ASMCL_ClassName { get; set; }
        public bool ASMCL_ActiveFlag { get; set; } 
        public long ASMS_Id { get; set; }
        public long AMST_Id { get; set; }
        public string AMST_FirstName { get; set; }
        public string AMST_AdmNo { get; set; }
        public studList1[] studList1 { get; set; }
        public long SPCCETM_Id { get; set; }
        public int count { get; set; }
        public int count1 { get; set; }
        public string msg { get; set; }
        public long SPCCE_Id { get; set; }
        public long SPCCMCL_Id { get; set; }
        public long SPCCMCC_Id { get; set; }
        public long SPCCMSCC_Id { get; set; }
        public string SPCCETM_TeamName { get; set; }
        public long SPCCETM_NoOfParticipants { get; set; }
        public bool SPCCETM_ActiveFlag { get; set; }
        public DateTime SPCCETM_CreatedDate { get; set; }
        public DateTime SPCCETM_UpdatedDate { get; set; }
        public long SPCCETM_CreatedBy { get; set; }
        public long SPCCETM_UpdatedBy { get; set; }
        public bool SPCCMH_ActiveFlag { get; set; }
        public TeamList[] TeamList { get; set; }
        public Array alldata { get; set; }
        public Array modalsponsorlist { get; set; }
        public string ASMAY_Year { get; set; }
        public string SPCCME_EventName { get; set; }
        public string SPCCMCL_CompitionLevel { get; set; }
        public string SPCCMCC_CompitionCategory { get; set; }
        public string SPCCMSCC_SportsCCName { get; set; }
        public Array editrecord { get; set; }
        public int SPCCMSCC_NoOfMembers { get; set; }
        public bool returnVal { get; set; }
        public string ASMC_SectionName { get; set; }

        // Praveen 13072023
        public Array teamlistone { get; set; }
        public long SPCCETMSCH_Id { get; set; }
        public string SPCCETMSCH_Team1 { get; set; }
        public string SPCCETMSCH_Team2 { get; set; }
        public string SPCCETMSCH_Time { get; set; }
        public DateTime SPCCETMSCH_Date { get; set; }
        public string SPCCETMSCH_Result { get; set; }
        public string SPCCETMSCH_Remarks { get; set; }
        public Array alldatas { get; set; }
        public Array geteditdata { get; set; }





        //public DateTime? fromdate { get; set; }
        //public DateTime? ToDate { get; set; }
        //public string Type { get; set; }
        //public Array GetReport { get; set; }
        //public Array gettsreport { get; set; }
        //public string logo { get; set; }
        //public string name { get; set; }
        //public Array GetMasterEvent { get; set; }
        //public long SPCCMSCCG_Id { get; set; }
        //public SubEventLis[] SubEventLists { get; set; }
    }
    public class TeamList
    {
        public long SPCCETM_Id { get; set; }
        public long AMST_Id { get; set; }
    }

}