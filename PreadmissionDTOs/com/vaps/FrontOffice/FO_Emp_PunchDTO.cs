using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.FrontOffice
{
    public class FO_Emp_PunchDTO
    {
        public long FOEP_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }

        public string emailId { get; set; }
        public long MobileNo { get; set; }

        public DateTime? FOEP_PunchDate { get; set; }
        public bool FOEP_HolidayPunchFlg { get; set; }
        public bool FOEP_Flag { get; set; }
        public string FOEPD_PunchTime { get; set; }   
        public string InOutFlg { get; set; }
        public string HRME_BiometricCode { get; set; }
        public string HRME_RFCardId { get; set; }
        public string FOEPD_Temperature { get; set; }
        public bool returnval { get; set; }
        public long FOEPD_Id { get; set; }
        public string FOEPD_InOutFlg { get; set; }
        public string ASPUD_InOutFlg { get; set; }
        public long FOBD_Id { get; set; }
        public DateTime? ASPUD_CreatedDate { get; set; }
        public DateTime? ASPUD_UpdatedDate { get; set; }
        public long? ASPUD_CreatedBy { get; set; }
        public long? ASPUD_UpdatedBy { get; set; }
        public string ASPUD_Flg { get; set; }

        public bool ASPU_ActiveFlg { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool ASPU_ManualEntryFlg { get; set; }
        public long ASPU_CreatedBy { get; set; }
        public long ASPU_UpdatedBy { get; set; }
        public FO_Emp_PunchDTO[] temp1 { get; set; }
        public string json { get; set; }

        public string EmployeeName { get; set; }
        public string InTime { get; set; }
        public string LateBy { get; set; }
        public string EntryTime { get; set; }
        public string OutTime { get; set; }
        public string ExitTime { get; set; }
        public string EarlyBy { get; set; }
        public Array Department_types { get; set; }
        public Array stf_types { get; set; }
        public Array Designation_types { get; set; }
        public Array holiday_types { get; set; }

        public Array employeelist { get; set; }
        public Array sfname { get; set; }
        public Array emplist { get; set; }
        public DateTime empdate { get; set; }
        public Array biometricdevicedetails { get; set; }

        //STUDENT PUNCHING
        public string AMST_BiometricId { get; set; }
        public DateTime? ASPU_PunchDate { get; set; }
        public string ASPUD_PunchTime { get; set; }
        public string FOBD_IPAddress { get; set; }
        //STUDENT PUNCHING
    }
}
