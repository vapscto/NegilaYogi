using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Reports
{
    public class NAACCriteriaFiveReportDTO : CommonParamDTO
    {

        public long NCAC512INSCH_Id { get; set; }
        public long NCAC512INSCH_Year { get; set; }
        public string NCAC512INSCH_SchemeName { get; set; }
        public long NCAC512INSCH_NoOfStudents { get; set; }
        public long NCAC523QE_NoOfStudentsappearing { get; set; }
        public long noofstdself { get; set; }
        public long ASMAY_Id { get; set; }
        public long UserId { get; set; }
        public long AMCST_Id { get; set; }
        public bool NCAC531SPCAS_FinancialORKindFlag { get; set; }
        public bool NCAC531SPCAS_DonationOfBooksFlag { get; set; }
        public bool NCAC531SPCAS_StudentsplacementFlag { get; set; }
        public bool NCAC531SPCAS_StudentexchangesFlag { get; set; }
        public bool NCAC531SPCAS_InstendowmentsFlag { get; set; }
        public long NCAC512NGSCH_Id { get; set; }
        public long NCAC512NGSCH_Year { get; set; }
        public string NCAC512NGSCH_SchemeName { get; set; }
        public long NCAC512NGSCH_NoOfStudents { get; set; }
        public long NCAC515VET_Id { get; set; }
        public long NCAC523QAMA_Id { get; set; }
        public long stud_count { get; set; }
        public long NCAC523QE_Id { get; set; }
        public long NCAC543ALMMET_Id { get; set; }
        public long NCAC542ALMCON_Id { get; set; }
        public long NCAC533SPCAA_Id { get; set; }
        public long NCAC521PLA_Id { get; set; }
        public long MI_Id { get; set; }
        public Array yearlist { get; set; }
        public Array nngovtsclist { get; set; }
        public Array nngovtsclistfiles { get; set; }
        public Array reportlist { get; set; }
        public Array examlist { get; set; }
        public Array getinstitutioncycle { get; set; }
        public long cycleid { get; set; }
        public string cyclename { get; set; }
        public string NCAC533SPCAA_ActType { get; set; }
        public int cycleorder { get; set; }
        public long NAACSL_Id { get; set; }
        public Array govtsclist { get; set; }
        public Array govtsclistfiles { get; set; }
        public Array instsclist { get; set; }
        public Array stdcntlist { get; set; }
        public Array instsclistfiles { get; set; }
        public CurricularAspects_DTO[] selectedYear { get; set; }
        public CurricularAspects_DTO[] yerlistdata { get; set; }
        public long NCAC513INSCH_Id { get; set; }
        public string NCAC513INSCH_DevSchemeName { get; set; }
        public string NCAC533SPCAA_ActLevel { get; set; }
        public long noofstd { get; set; }
        public string NCAC513INSCH_AgencyDetails { get; set; }
        public string ASMAY_Year { get; set; }
        public string ASMAY_Year1 { get; set; }
        public string aadharpan { get; set; }
        public string institutionname { get; set; }
        public string NCAC516GRI_GRIAPP { get; set; }
        public string NCAC516GRI_GRIRED { get; set; }
        public string NCAC516GRI_AvgTime { get; set; }
        public string AMB_BranchName { get; set; }
        public string AMCO_CourseName { get; set; }
        public string NCAC521PLA_EmployerName { get; set; }
        public string amount { get; set; }
        public string AMCST_FirstName { get; set; }
        public long NCAC522HRED_Id { get; set; }
        public string atudentname { get; set; }
        public string department { get; set; }
        public string program { get; set; }
        public string awardname { get; set; }
        public string NCAC531SPCAS_NatOrInterNatFlg { get; set; }
        public string AMCST_RegistrationNo { get; set; }
        public string NCAC531SPCAS_SportsCAIEEEFlg { get; set; }
        public long? AMCST_AadharNo { get; set; }
        public long NCAC531SPCA_Id { get; set; }
        public long NCAC531SPCAS_Id { get; set; }
        public Array getinstitution { get; set; }
        public long NCAC511GSCH_Id { get; set; }
        public long NCAC511GSCH_Year { get; set; }
        public string NCAC511GSCH_SchemeName { get; set; }
        public long NCAC511GSCH_NoOfStudents { get; set; }
        public CurricularAspects_DTO[] selected_Inst { get; set; }
        public string NAACSL_InstitutionTypeFlg { get; set; }
        public int ASMAY_Order { get; set; }
        public long NCAC543ALMMET_NoOfMeetings { get; set; }
        public DateTime NCAC543ALMMET_MeetingDate { get; set; }
        public long NCAC543ALMMET_NoOfMemAttnd { get; set; }
        public long NCAC543ALMMET_TotalAlumniCount { get; set; }
        public bool NCAC516GRI_AdpOfguidelinesofRegbodiesFlg { get; set; }
        public bool NCAC516GRI_StusgrvOnline_OR_offlineFlg { get; set; }
        public bool NCAC516GRI_CommitteewithminutesFlg { get; set; }
        public bool NCAC516GRI_RecordOfActionTakenFlg { get; set; }

    }
}
