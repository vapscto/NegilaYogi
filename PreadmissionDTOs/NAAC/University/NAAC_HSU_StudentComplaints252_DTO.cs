using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.University
{
  public class NAAC_HSU_StudentComplaints252_DTO
    {

        public long NCHSU252SC_Id { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long NCHSU252SC_Year { get; set; }
        public long NCHSU252SC_NoOfStudentsComplaints { get; set; }
        public long NCHSU252SC_TotalNoOfStudentsAppereadExam { get; set; }
        public bool NCHSU252SC_ActiveFlag { get; set; }
        public DateTime NCHSU252SC_CreatedDate { get; set; }
        public DateTime NCHSU252SC_UpdatedDate { get; set; }
        public long NCHSU252SC_CreatedBy { get; set; }
        public long NCHSU252SC_UpdatedBy { get; set; }
        public Array institutionlist { get; set; }
        public Array allacademicyear { get; set; }
        public NAAC_HSU_StudentComplaints252_DTO[] filelist { get; set; }
        public Array alldata1 { get; set; }
        public string ASMAY_Year { get; set; }
        public bool returnval { get; set; }
        public long ASMAY_Id { get; set; }
        public bool ret { get; set; }
        public long NCHSU252SCF_Id { get; set; }

        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }
        public string msg { get; set; }
        public Array editFileslist { get; set; }
        public Array editlist { get; set; }
        public Array viewuploadflies { get; set; }
    }
}
