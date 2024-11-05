using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class StudentIdCardFormatDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long UserId { get; set; }
        public long AMST_Id { get; set; }
        public string StudentName { get; set; }
        public Array GetAcademicYearList { get; set; }
        public Array GetClassList { get; set; }
        public Array GetSectionList { get; set; }
        public Array GetStudentList { get; set; }
        public Array cardData { get; set; }
        public string retrunMsg1 { get; set; }
        public string retrunMsg { get; set; }
        public StudentTempList[] StudentTempList { get; set; }
    }
    public class StudentTempList
    {
        public long AMST_Id { get; set; }
        public string StudentName { get; set; }
    }
}

