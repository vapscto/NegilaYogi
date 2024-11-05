using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.University
{
   public class MC_314_ResearchAssociatesDTO
    {
        public long NCMCRA314_Id { get; set; }
        public long NCMCRA314F_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public long UserId { get; set; }
        public long NCMCRA314_Year { get; set; }
        public string NCMCRA314_NameOfResearch { get; set; }
        public string NCMCRA314_Type { get; set; }
        public string NCMCRA314_GrantingAgency { get; set; }
        public string NCMCRA314_QualExamName { get; set; }
        public string NCMCRA314_Duration { get; set; }
        public bool NCMCRA314_ActiveFlag { get; set; }
        public long NCMCRA314_CreatedBy { get; set; }
        public long NCMCRA314_UpdatedBy { get; set; }
        public DateTime? NCMCRA314_CreatedDate { get; set; }
        public DateTime? NCMCRA314_UpdatedDate { get; set; }

        public Array allacademicyear { get; set; }
        public Array alldata1 { get; set; }
        public Array institutionlist { get; set; }
        public string ASMAY_Year { get; set; }
        public long asmaY_Id { get; set; }
        public bool duplicate { get; set; }
        public MC_314_ResearchAssociatesDTO[] filelist { get; set; }
        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }
        public string msg { get; set; }
        public bool returnval { get; set; }
        public Array editlist { get; set; }
        public Array editFileslist { get; set; }
        public Array viewuploadflies { get; set; }
       


    }
}
