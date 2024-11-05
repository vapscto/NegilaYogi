using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.University
{
  public  class HSU_352_RevenueGeneratedDTO
    {
        public long NCMCRG352_Id { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long NCMCRG352F_Id { get; set; }
        public long NCMCRG352_Year { get; set; }
        public string NCMCRG352_ConsultantName { get; set; }
        public string NCMCRG352_AdvisoryName { get; set; }
        public string NCMCRG352_ConsultingORSpnAgencyCD { get; set; }
        public decimal NCMCRG352_RevenueGeneratedAmount { get; set; }
        public long NCMCRG352_CreatedBy { get; set; }
        public long NCMCRG352_UpdatedBy { get; set; }
        public bool NCMCRG352_ActiveFlag { get; set; }
        public DateTime? NCMCRG352_CreatedDate { get; set; }
        public DateTime? NCMCRG352_UpdatedDate { get; set; }

        public Array allacademicyear { get; set; }
        public Array alldata1 { get; set; }
        public Array institutionlist { get; set; }
        public string ASMAY_Year { get; set; }
        public long asmaY_Id { get; set; }
        public bool duplicate { get; set; }
        public HSU_352_RevenueGeneratedDTO[] filelist { get; set; }
        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }
        public string msg { get; set; }
        public bool returnval { get; set; }
        public Array editlist { get; set; }
        public Array editFileslist { get; set; }
        public Array viewuploadflies { get; set; }
        public long NCMC8110IMMF_Id { get; set; }
    }
}
