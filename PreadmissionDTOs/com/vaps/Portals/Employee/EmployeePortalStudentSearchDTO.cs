using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Portals.Employee
{
    public class EmployeePortalStudentSearchDTO
    {
        public class Input
        {
            public long MI_Id { get; set; }
            public long ASMAY_Id { get; set; }
            public long ASMS_Id { get; set; }
            public long ASMCL_Id { get; set; }
            public long AMST_Id { get; set; }
           

        }
        public class Output
        {
            public Array studentList { get; set; }
            public Array classlist { get; set; }
            public Array sectionList { get; set; }
            public Array examlist { get; set; }
            public Array fillstudentalldetails { get; set; }


        }
        public class Output_StudentList
        {
            public long AMST_Id { get; set; }
            public string AMST_FirstName { get; set; }

        }
        public class Output_SectionList
        {
            public long ASMS_Id { get; set; }
            public string sectionname { get; set; }

        }
        public class Output_ClassList
        {
            public long ASMCL_Id { get; set; }
            public string classname { get; set; }

        }
        public class Output_StudentD
        {
            public string amst_FirstName { get; set; }
            public string amst_RegistrationNo { get; set; }
            public string amst_AdmNo { get; set; }
            public long amay_RollNo { get; set; }
            public string classname { get; set; }
            public string sectionname { get; set; }
            public string fathername { get; set; }
            public string mothername { get; set; }
            public string bloodgroup { get; set; }
            public string address1 { get; set; }
            public string address2 { get; set; }
            public string address3 { get; set; }
            public long? fathermobileno { get; set; }
            public long amst_mobile { get; set; }
            public string amst_sex { get; set; }
            public DateTime amst_dob { get; set; }
            public string amst_emailid { get; set; }
            public string asma_year { get; set; }
            public DateTime studentdob { get; set; }
        }

        public class Output_StudentExamD
        {
            public string exam_name { get; set; }
            public int EME_Id { get; set; }
            public decimal? totalmarks { get; set; }
            public decimal? obtainmarks { get; set; }
            public decimal? persentage { get; set; }
            public string result { get; set; }
        }
            public Array EmployeePortalStudentSearchD { get; set; }
    }
}
