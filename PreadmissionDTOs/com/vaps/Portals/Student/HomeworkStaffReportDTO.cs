using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Portals.Student
{
    public class HomeworkStaffReportDTO
    {

        public long MI_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long HRME_Id { get; set; }
        public long USER_ID { get; set; }
        public long IHW_Id { get; set; }
        public long ASMCC_Id { get; set; }
        public string ASMC_SectionName { get; set; }
        public Array Section_list { get; set; }
        public string roleType { get; set; }
        public string ASMCL_ClassName { get; set; }
        public long ISMS_Id { get; set; }
        public string IHWUPL_FileName { get; set; }
        public Array Select_list { get; set; }

        public string HRME_EmployeeFirstName { get; set; }

        public Array Empl_name { get; set; }


        public DateTime? IHW_DateFrom { get; set; }


        public DateTime? IHW_DateTo { get; set; }

        public Array getReport { get; set; }

        public Array getview { get; set; }
        public Array classlist { get; set; }
        public DateTime? IHW_Date { get; set; }

        public DateTime? IHWUPL_Date { get; set; }

        public sectionselect[] ASMSId_Filter { get; set; }
        public Array academicdrp { get; set; }

        //public sectionselect[] sectionselect { get; set; }
        public smssent[] studentsms { get; set; }
        public emailsent[] studentemail { get; set;}
        public string flagstring { get; set; }
    }

    public class sectionselect
    {
        public long ASMS_Id { get; set; }
    }
    public class smssent
    {
        public long? AMST_Id { get; set; }
        public string studentname { get; set; }
    }
    public class emailsent
    {
        public long? AMST_Id { get; set; }
        public string studentname { get; set; }
    }
}
