using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.University
{
   public class NAAC_HSU_345_TeacherResearchPapers_DTO
    {

        public long NCHSU345TRP_Id { get; set; }
        public long NCHSU345TRPF_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCHSU345TRP_Year { get; set; }
        public string NCHSU345TRP_PaperTitle { get; set; }
        public string NCHSU345TRP_AuthorName { get; set; }
        public string NCHSU345TRP_DepartmentName { get; set; }
        public string NCHSU345TRP_JournalName { get; set; }
        public string NCHSU345TRP_ISSNNumber { get; set; }
        public string NCHSU345TRP_link { get; set; }
        public string NCHSU345TRP_NamesOfIndexingDatabases { get; set; }
        public bool NCHSU345TRP_ActiveFlag { get; set; }
        public long NCHSU345TRP_CreatedBy { get; set; }
        public long NCHSU345TRP_UpdatedBy { get; set; }
        public DateTime NCHSU345TRP_CreatedDate { get; set; }
        public DateTime NCHSU345TRP_UpdatedDate { get; set; }





        public Array list { get; set; }
        public Array allacademicyear { get; set; }
        public Array alldata1 { get; set; }
        public string ASMAY_Year { get; set; }
        public string msg { get; set; }
        public long UserId { get; set; }
        public long ASMAY_Id { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public bool ret { get; set; }
        public Array editlist { get; set; }
        public NAAC_HSU_345_TeacherResearchPapers_DTO[] filelist { get; set; }
        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }
        public Array editFileslist { get; set; }
        public Array viewuploadflies { get; set; }
     
        public Array institutionlist { get; set; }


    }
}
