using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.College.BirthDay
{
    public class ClgBirthDayDTO
    {
        public bool returnval { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string typeflag { get; set; }
        public bool? SMSFlag { get; set; }
        public bool? EmailFlag { get; set; }
        public Array birthdaylist { get; set; }
        public CLGBithdaySelectedListDTO[] selectedarray { get; set; }

        //===================================REPORT
        public Array fillyear { get; set; }
        public Array fillmonth { get; set; }
        public int monthid { get; set; }
        public string monthname { get; set; }
        public string ASMAY_Year { get; set; }
        public Array sms_mail_count { get; set; }
        public Array studentDetails { get; set; }
        public Array staffList { get; set; }
        public Array classDrpDwn { get; set; }
        public Array sectionDrpDwn { get; set; }
        public Array studentlist { get; set; }
        public long AMST_MobileNo { get; set; }
        public string AMST_emailId { get; set; }
        public int count { get; set; }
        public DateTime? AMST_DOB { get; set; }
        public long AMST_Id { get; set; }
        public string studentname { get; set; }
        public int month { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }

        public long HRME_Id { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string HRME_EmployeeMiddleName { get; set; }
        public string HRME_EmployeeLastName { get; set; }
        public bool? HRME_LeftFlag { get; set; }
        public long HRME_MobileNo { get; set; }
        public string HRME_EmailId { get; set; }
        public DateTime? HRME_DOB { get; set; }
        public string employeename { get; set; }
        public DateTime? date11 { get; set; }
        public DateTime? date12 { get; set; }
        public long ssb { get; set; }
        public long esb { get; set; }
        public string smsStatus { get; set; }
        public string year { get; set; }
        public Array Month_array { get; set; }
        public Array acayear { get; set; }
        public string employeeName { get; set; }

    }
    public class CLGBithdaySelectedListDTO
    {
        public long AMCST_Id { get; set; }
        public string AMCST_AdmNo { get; set; }
        public long AMCST_MobileNo { get; set; }
        public string AMCST_emailId { get; set; }
        public DateTime? AMCST_DOB { get; set; }

        public long HRME_Id { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public long HRME_MobileNo { get; set; }
        public string HRME_EmailId { get; set; }
        public DateTime? HRME_DOB { get; set; }




    }
}
