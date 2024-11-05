using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Sports
{
    public class StudentAgeCalcDTO : CommonParamDTO
    {
        public long SPCCAC_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public int Age_Years { get; set; }
        public int Age_Months { get; set; }
        public int Age_Days { get; set; }
        public DateTime Till_Date { get; set; }
        public Array academicYear { get; set; }
        public Array classList { get; set; }
        public Array sectionList { get; set; }
        public string ASMAY_Year { get; set; }
        public string studentName { get; set; }
        public Array studentList { get; set; }
        public StudentAgeCalcDTO[] student { get; set; }
        public string returnVal { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public Array eventsStudentRecordList { get; set; }
        public int count { get; set; }
        public Array datareport { get; set; }
        public string AMST_AdmNo { get; set; }
        public DateTime AMST_DOB { get; set; }

        public string stud { get; set; }
        public Array studage { get; set; }
        public DateTime AMST_Date { get; set; }

        public Array houseList { get; set; }
    }
}
