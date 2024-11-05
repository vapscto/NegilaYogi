using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class ExamLoginPrivilegesReportDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public int EMCA_Id { get; set; }
        public long HRME_Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public bool HRME_ActiveFlag { get; set; }
        public Array Acdlist { get; set; }
        public Array ctlist { get; set; }
        public Array seclist { get; set; }
        public Array catlist { get; set; }
        public Array stafflist { get; set; }
        public Array datareport { get; set; }
        public Array institution { get; set; }
        public string ISMS_SubjectName { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string HRME_EmployeeMiddleName { get; set; }
        public string HRME_EmployeeLastName { get; set; }
        public string EmployeeName { get; set; }
        public string ClassName { get; set; }
        public string SubjectName { get; set; }
        public string SectionName { get; set; }
        public string report_type { get; set; }
        public int check_type { get; set; }
    }
}
