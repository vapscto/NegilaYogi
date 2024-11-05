using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class NAACHRMasterEmpFullTimeDTO
    {
        public long HRMEPT_Id { get; set; }
        public long HRME_Id { get; set; }
        public long HRMEPT_Year { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public bool HRMEPT_ActiveFlag { get; set; }
        public Array empdata { get; set; }
        public Array employeelist { get; set; }
        public Array academicyearlist { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string ASMAY_Year { get; set; }
        public DateTime? ASMAY_From_Date { get; set; }
        public DateTime? ASMAY_To_Date { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public string msg { get; set; }
        public Array fulltimedetailList { get; set; }
        public selectedEmp[] selectedEmp { get; set; }
        public int count { get; set; }
        public int count1 { get; set; }
    }
    public class selectedEmp
    {
        public long HRME_Id { get; set; }
    }
}