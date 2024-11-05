using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Exam.StudentMentor
{
    public class SchoolstudentmentormappingDTO
    {
        public long AMMEM_Id { get; set; }
        public long AMME_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long AMST_Id { get; set; }
        public long AMME_CreatedBy { get; set; }
        public long AMME_UpdatedBy { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long HRME_Id { get; set; }
        public long AMMEM_CreatedBy { get; set; }
        public long AMMEM_UpdatedBy { get; set; }
        public Array getyear { get; set; }
        public Array getclass { get; set; }
        public Array getsection { get; set; }
        public Array getemployee { get; set; }
        public Array getdetails { get; set; }
        public Array getstudentdetails { get; set; }
        public Array getsaveddetails { get; set; }
        public Array getstudentdata { get; set; }
        public Array getreportdata { get; set; }
        public string message { get; set; }
        public bool returnval { get; set; }
        public long Userid { get; set; }
        public string ASMAY_Year { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public string employeename { get; set; }
        public SchoolstudentmentormappingtempDTO[] SchoolstudentmentormappingtempDTO { get; set; }

    }
    public class SchoolstudentmentormappingtempDTO
    {
        public long AMST_Id { get; set; }
    }
}
