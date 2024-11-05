using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Medical
{
   public class NAAC_MC_436_EContent_DTO
    {
        public long NCMCMEC436_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public string NCMCMEC436_ModuleName { get; set; }
        public string NCMCMEC436_PlatformModuleUsed { get; set; }
        public DateTime? NCMCMEC436_Date { get; set; }
        public string NCMCMEC436_WebLink { get; set; } 
        public long NCMCMEC436F_Id { get; set; }
        public string NCAC434ECTF_FileName { get; set; }
        public string NCAC434ECTF_Filedesc { get; set; }
        public string NCAC434ECTF_FilePath { get; set; }

        public long UserId { get; set; }
        public long ASMAY_Id { get; set; }
        public string ASMAY_Year { get; set; }
        public string msg { get; set; }
        public bool returnval { get; set; }
        public int count { get; set; }
        public int count1 { get; set; }
        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }
        public string empname { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public int? HRME_EmployeeOrder { get; set; }
        public long NCMCMEC436_Year { get; set; }
        public bool NCMCMEC436_ActiveFlag { get; set; }

        public Array allgridlist { get; set; }
        public Array editlist { get; set; }
        public Array institutionlist { get; set; }
        public Array editFileslist { get; set; }
        public Array viewuploadflies { get; set; }
        public Array yearlist { get; set; }
        public Array emplylist { get; set; }
      
        public Naac_CommonFiles_DTO[] filelist { get; set; }
        public empchecklist[] empchecklist { get; set; }

    }
    public class empchecklist
    {
        public long HRME_Id { get; set; }
    }
}
