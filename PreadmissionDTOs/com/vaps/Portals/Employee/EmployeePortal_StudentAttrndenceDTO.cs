using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Portals.Employee
{
    public class EmployeePortal_StudentAttrndenceDTO
    {
        public class Input
        {
            public long MI_Id { get; set; }
            public long HRME_Id { get; set; }
            public long ASMAY_Id { get; set; }
            public int type { get; set; }
             public long AMST_Id { get; set; }
            public long ASMCL_Id { get; set; }
            public long ASMS_Id { get; set; }
        }
        public class Output
        {
           
            public Array fillmonths { get; set; }

            public string monthName { get; set; }
            public decimal present { get; set; }
            public decimal classheld { get; set; }
            public decimal perc { get; set; }

            public Array attendencelist { get; set; }
            public Array allstudent { get; set; }
            public Array studentList { get; set; }
            public Array classlist { get; set; }
            public Array sectionList { get; set; }
            public Array yearlist { get; set; }
        }
        public class OutputAllStudent
        {
            public long AMST_Id { get; set; }
            public string studentname { get; set; }
            public Int64 monthid { get; set; }
            public string monthName { get; set; }
            public decimal present { get; set; }
            public decimal classheld { get; set; }
            public decimal perc { get; set; }
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


        public Array EmployeePortal_StudentAttrndence { get; set; }
    }
}
