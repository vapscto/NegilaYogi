using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.University
{
   public class NAAC_HSU_InterdisciplinaryProgrammes_123_DTO
    {
        public long NCHSUIP123_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCHSUIP123_Year { get; set; }
        public long NCHSUIP123_TotalNoOfProg { get; set; }
        public long NCHSUIP123_TotalNoOfCoursesAcrossProgs { get; set; }
        public long NCHSUIP123_NoOfInterdisciplinaryCoursesAcsProg { get; set; }
        public bool NCHSUIP123_ActiveFlag { get; set; }
        public long NCHSUIP123_CreatedBy { get; set; }
        public long NCHSUIP123_UpdatedBy { get; set; }
        public DateTime NCHSUIP123_CreatedDate { get; set; }
        public DateTime NCHSUIP123_UpdatedDate { get; set; }
        public Array institutionlist { get; set; }
        public NAAC_HSU_InterdisciplinaryProgrammes_123_DTO[] filelist { get; set; }
        public bool returnval { get; set; }
        public long ASMAY_Id { get; set; }
        public Array alldata1 { get; set; }
        public string ASMAY_Year { get; set; }
        public Array allacademicyear { get; set; }
        public long NCHSUIP123F_Id { get; set; }
        public long UserId { get; set; }
        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }
        public string msg { get; set; }
        public Array editFileslist { get; set; }
        public bool ret { get; set; }
        public Array editlist { get; set; }
        public Array viewuploadflies { get; set; }
    }
}
