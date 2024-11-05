using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Medical
{
    public class NAAC_MC_423_StuLearningResource_DTO
    {
        public long UserId { get; set; }
        public long MI_Id { get; set; }
        public Array institutionlist { get; set; }
        public Array allacademicyear { get; set; }
        public Array alldata1 { get; set; }
        public long NCMCI423SLR_Id { get; set; }
        public long NCMCI423SLRF_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long NCMCI423SLR_Year { get; set; }
        public string NCMCI423SLR_ResourcesName { get; set; }
        public string ASMAY_Year { get; set; }
        public bool returnval { get; set; }
        public long NCMCI423SLR_NoOfUGStudentsExposed { get; set; }
        public long NCMCI423SLR_NoOfPGStudentsExposed { get; set; }
        public NAAC_MC_423_StuLearningResource_DTO[] filelist {get;set;}
        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }
        public Array editFileslist { get; set; }
        public Array viewuploadflies { get; set; }
        public Array editlist { get; set; }
        public string msg { get; set; }






        public long NCMCI424_Id { get; set; }
  
        public long NCMCI424_Year { get; set; }
        public bool NCMCI424_AttchSatellitePrimaryHealthCenterFlag { get; set; }
        public bool NCMCI424_AttchRuralHealthCenterFlag { get; set; }
        public bool NCMCI424_ResFacilityForStudentsORtraineesFlag { get; set; }
        public long NCMCI424_CreatedBy { get; set; }
        public long NCMCI424_UpdatedBy { get; set; }
        public DateTime NCMCI424_CreateDate { get; set; }
        public DateTime NCMCI424_UpdatedDate { get; set; }
        public bool NCMC423ICBL_AttcurbanHCTrainingOfStudentsFlag { get; set; }
        public string flag { get; set; }
    }
}
