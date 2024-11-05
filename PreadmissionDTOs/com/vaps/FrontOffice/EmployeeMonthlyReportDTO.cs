using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.FrontOffice
{
    public class EmployeeMonthlyReportDTO
    {
        public long MI_Id { get; set; }
        public Array filltypes { get; set; }
        public Array filldepartment { get; set; }
        public Array filldesignation { get; set; }
        public Array fillemployee { get; set; }
        public Array filldata { get; set; }
        public Array employeelist { get; set; }
        public string multipletype { get; set; }
        public string multipledep { get; set; }
        public string multipledes { get; set; }
        public string multiplehrmeid { get; set; }
        
        public long HRMD_Id { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public long HRMDES_Id { get; set; }
        public string HRMDES_DesignationName { get; set; }
        public long HRME_Id { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string HRME_EmployeeMiddleName { get; set; }
        public string HRME_EmployeeLastName { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public DateTime HRME_DOJ { get; set; }
        public string fromdate { get; set; }
        public string todate { get; set; }
        public string columnnames { get; set; }
        public string totalworkingdays { get; set; }
        public string type { get; set; }
        public Array filldataS { get; set; }

        public string columnnamesD { get; set; }

        public string totalworkingdaysD { get; set; }
    }
}
