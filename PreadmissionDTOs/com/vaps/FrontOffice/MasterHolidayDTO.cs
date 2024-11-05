using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.FrontOffice
{
    public class MasterHolidayDTO:CommonParamDTO
    {
        public int _frnt;
        public string message;
        public int FOMHWD_Id { get; set; }
        public long MI_Id { get; set; }
        public int FOHWDT_Id { get; set; }
        public string FOMHWD_HolidayWDName { get; set; }
        // public string FOMHWD_CalenderYear { get; set; }
        public bool FOMHWD_ActiveFlg { get; set; }
        public Array yeardropdown { get; set; }
        public DateTime? FO_Start_Date { get; set; }
        public DateTime? FO_End_Date { get; set; }
        public string FO_Holiday_name { get; set; }
        public string FO_Remark { get; set; }
        public string FOHTWD_HolidayWDType { get; set; }
        public string FOHTWD_HolidayWDTypeFlag { get; set; }
        public bool FOHWDT_ActiveFlg { get; set; }
        public int FOMHWDD_Id { get; set; }
        public DateTime? FOMHWDD_Date { get; set; }
        public bool returnval { get; set; }

        public Array gridviewdetails { get; set; }
        public Array holidayType { get; set; }

        public Array dayslist { get; set; }
        public Array departmentType { get; set; }
        public long HRMD_Id { get; set; }


        public bool Day_flag { get; set; }
        public TT_Master_DayDTO[] daylists { get; set; }
        public FODayNameDTO[] daylists1 { get; set; }
        public string ASMAY_Year { get; set; }
        public DateTime? ASMAY_From_Date { get; set; }
        public DateTime? ASMAY_To_Date { get; set; }
        public Array report_list { get; set; }
        public long HRMLY_Id { get; set; }
        public string Date { get; set; }
        public string Dayname { get; set; }
        public Array datesanddayList { get; set; }
        public MasterHolidayDTO[] selectedDaysDate { get; set; }
        public int count { get; set; }
        public bool FOHTWD_HolidayFlag { get; set; }
        public DateTime? FOMHWDD_FromDate { get; set; }
        public DateTime? FOMHWDD_ToDate { get; set; }
        public string FOMHWDD_Name { get; set; }
        public Array employeelist { get; set; }
        public long HRME_Id { get; set; }
        public DateTime? FOMEH_Date { get; set; }
        public string FOMEH_Day { get; set; }
        public long FOMEH_Id { get; set; }
        public long LogInId { get; set; }
        public bool FOMEH_ActiveFlg { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public long Userid { get; set; }
    }
}
