using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.FrontOffice
{
    public class EmployeeShiftMappingDTO
    {
        public int FOMTS_Id { get; set; }
        public string FOMTS_FDWHrMin { get; set; }

        public string FOMTS_HDWHrMin { get; set; }
        public string FOMTS_IHalfLoginTime { get; set; }
        public string FOMTS_IhalfLogoutTime { get; set; }
        public string FOMTS_IIHalfLoginTime { get; set; }
        public string FOMTS_IIHalfLogoutTime { get; set; }
        public string FOMTS_DelayPerShiftHrMin { get; set; }
        public string FOMTS_EarlyPerShiftHrMin { get; set; }
        public string FOMTS_LunchHoursDuration { get; set; }
        public string FOMTS_BlockAttendance { get; set; }
        public string FOMTS_FixTimings { get; set; }
        public bool FOMHWD_ActiveFlg { get; set; }
        public long MI_Id { get; set; }


        public int FOEST_Id { get; set; }
        public long HRME_Id { get; set; }
        public int FOHWDT_Id { get; set; }
        public string FOEST_FDWHrMin { get; set; }
        public string FOEST_HDWHrMin { get; set; }
        public string FOEST_IHalfLoginTime { get; set; }
        public string FOEST_IHalfLogoutTime { get; set; }
        public string FOEST_IIHalfLoginTime { get; set; }
        public string FOEST_IIHalfLogoutTime { get; set; }
        public string FOEST_DelayPerShiftHrMin { get; set; }
        public string FOEST_EarlyPerShiftHrMin { get; set; }
        public string FOEST_LunchHoursDuration { get; set; }
        public string FOEST_BlockAttendance { get; set; }
        public string FOEST_FixTimings { get; set; }

        public int FOMS_Id { get; set; }
        public string FOMS_ShiftName { get; set; }
        public bool FOMS_ActiveFlg { get; set; }


        public int FOMST_Id { get; set; }
        public string FOMST_FDWHrMin { get; set; }
        public string FOMST_HDWHrMin { get; set; }
        public string FOMST_IHalfLoginTime { get; set; }
        public string FOMST_IHalfLogoutTime { get; set; }
        public string FOMST_IIHalfLoginTime { get; set; }
        public string FOMST_IIHalfLogoutTime { get; set; }
        public string FOMST_DelayPerShiftHrMin { get; set; }
        public string FOMST_EarlyPerShiftHrMin { get; set; }
        public string FOMST_LunchHoursDuration { get; set; }
        public string FOMST_BlockAttendance { get; set; }
        public string FOMST_FixTimings { get; set; }

        public long HRMD_Id { get; set; }
        public string HRMD_DepartmentName { get; set; }

        public bool returnval { get; set; }
        public string returnduplicatestatus { get; set; }
        public string returnsavestatus { get; set; }
        public string returnupdatestatus { get; set; }
        public Array stf_types { get; set; }

        public Array Department_types { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string FOHTWD_HolidayWDType { get; set; }

        public long HRMDES_Id { get; set; }
        public string HRMDES_DesignationName { get; set; }

        public long HRMGT_Id { get; set; }

        public string HRMGT_EmployeeGroupType { get; set; }

        public Array Designation_types { get; set; }
        public Array holiday_types { get; set; }
        public Array sfname { get; set; }
        public Array sflist { get; set; }
        public Array employeelist { get; set; }
        public Array emplist { get; set; }
        public Array emplist1 { get; set; }
        public Array editlist { get; set; }
        public Array get_emp { get; set; }
        public EmployeeShiftMappingDTO[] emptypes { get; set; }
        public EmployeeShiftMappingDTO[] empdept { get; set; }
        public EmployeeShiftMappingDTO[] empdesg { get; set; }
        public EmployeeShiftMappingDTO[] employee { get; set; }
        public EmployeeShiftMappingDTO[]  SelectedDayType {get;set;}
        public DateTime? FOEST_Date { get; set; }
        public long Userid { get; set; }
    }
}
