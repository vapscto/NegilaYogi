using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Medical
{
   public class NAAC_MC_VACcommon_DTO
    {
        public long NCMCVAC141_Id { get; set; }
        public long MI_Id { get; set; }
        public bool NCMCVAC141_FKFromStudents { get; set; }
        public bool NCMCVAC141_FKFromteachers { get; set; }
        public bool NCMCVAC141_FKFromemployers { get; set; }
        public bool NCMCVAC141_FKFromalumni { get; set; }
        public bool FkCollFromOtherProfs { get; set; }
        public DateTime NCMCVAC141_CreatedDate { get; set; }
        public DateTime NCMCVAC141_UpdatedDate { get; set; }
        public long NCMCVAC141_CreatedBy { get; set; }
        public long NCMCVAC141_UpdatedBy { get; set; }
        public long NCMCVAC141_year { get; set; }
        public bool NCMCVAC142_FKNotcollected { get; set; }

        public long NCMCVAC142_Id { get; set; }
        public long NCMCS232_Id { get; set; }
        public bool NCMCVAC142_FKCollAnlInstWebsite { get; set; }
        public bool NCMCVAC142_FKCollAnlFk { get; set; }
        public bool NCMCVAC142_FKCollanalysed { get; set; }
        public bool NCMCVAC142_FKcollected { get; set; }
        public DateTime NCMCVAC142_CreatedDate { get; set; }
        public DateTime NCMCVAC142_UpdatedDate { get; set; }
        public long NCMCVAC142_CreatedBy { get; set; }
        public long NCMCVAC142_UpdatedBy { get; set; }
        public long NCMCVAC142_year { get; set; }

        public long NCMCM221_Id { get; set; }        
        public long NCMCM221_Year { get; set; }
        public bool NCMCM221_MesCrFolldRegSlowPerFlag { get; set; }
        public bool NCMCM221_MesCrFolldAdLersFlag { get; set; }
        public bool NCMCM221_SpecialprogCrLowORAdlersFlag { get; set; }
        public bool NCMCM221_ProclsMeasureAchsFlag { get; set; }
        public long UserId { get; set; }
        public long ASMAY_Id { get; set; }
        public string ASMAY_Year { get; set; }

        public long NCMCCI254_Id { get; set; }
        public long NCMCS232_Year { get; set; }
        public long NCMCCI254_Year { get; set; }
        public bool NCMCCI254_TimelyAdministrationCIEFlag { get; set; }
        public bool NCMCCI254_OnTimeAssessmentFeedbackFlag { get; set; }
        public bool NCMCCI254_MakeupAssignmentsFlag { get; set; }
        public bool NCMCCI254_RemedialTeachingFlag { get; set; }

        public bool duplicate { get; set; }
        public bool returnval { get; set; }

        public bool NCMCS232_InstClinicalSkillsFlag { get; set; }
        public bool NCMCS232_InstAdvsimulationBasedTrainingFlag { get; set; }
        public bool NCMCS232_StuProgTrAsstofStudentsFlag { get; set; }
        public bool NCMCS232_StuProgTrAsstClORSimulationLrnFlag { get; set; }

        public Array editdata { get; set; }
        public Array institutionlist { get; set; }
        public Array yearlist { get; set; }
        public Array alldata141 { get; set; }
        public Array alldata142 { get; set; }
        public Array alldata221 { get; set; }
        public Array alldata232 { get; set; }
        public Array alldata254 { get; set; }
    }
}
