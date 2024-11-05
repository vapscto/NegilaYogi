using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.FrontOffice
{
    public class EmployeeInOutReportDTO
    {       

        public long MI_Id { get; set; }
        public Array filltypes { get; set; }
        public Array filldepartment { get; set; }
        public Array filldesignation { get; set; }
        public Array fillemployee { get; set; }
        public Array filldata { get; set; }
        public string multipletype { get; set; }
        public string multipledep { get; set; }
        public string multipledes { get; set; }
        public string multipleemp { get; set; }
        public long HRMD_Id { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public long HRMDES_Id { get; set; }
        public string HRMDES_DesignationName { get; set; }
        public long HRME_Id { get; set; }
        public string ename { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public string fromdate { get; set; }
        public string todate { get; set; }
        public Array columnnames { get; set; }
        public string multiplehrmeid { get; set; }
        public DateTime? FOEP_PunchDate { get; set; }
        public string punchtype { get; set; }
        public string ecode { get; set; }
        public string FOEPD_InOutFlg { get; set; }
        public string FOEPD_PunchTime { get; set; }
        public Array activityIds { get; set; }
        public Array filldataLIEO { get; set; }
        public int FOEST_IHalfLoginTime { get; set; }
        public int? HRME_EmployeeOrder { get; set; }
        //public bool lateinflg { get; set; }
        //public bool earlyoutflg { get; set; }
        //public decimal inouttimes { get; set; }
    }
}
