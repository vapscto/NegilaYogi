using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Portals.Principal
{
    public class PrincipalDashboardDTO : CommonParamDTO
    {
        public int EME_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long Userid { get; set; }
        public long AMST_Id { get; set; }
        public long roleid { get; set; }
        public long IVRMUL_Id { get; set; }
        public Array attachementlist { get; set; }
        public Array attachementlist1 { get; set; }
        public long PaymentNootificationPrinicipal { get; set; }
        public Array Smscount { get; set; }
        public Array getpaymentnotificationdetails { get; set; }
        public Array Emailcount { get; set; }
        public long totalsms { get; set; }
        public long totalEmail { get; set; }
        public Array Fillstudentstrenth { get; set; }
        public Array yearlist { get; set; }
        public Array fillabsent { get; set; }
        public Array fillfee { get; set; }
        public Array stdabsentlist { get; set; }
        public Array studentbrthlist { get; set; }
        public Array classteacherlst { get; set; }
        public Array subjecttealst { get; set; }
        public Array lateinlst { get; set; }
        public Array earlyoutlst { get; set; }
        public Array absentlst { get; set; }
        public long INTB_Id { get; set; }
        public string feeclass { get; set; }
        public string Class_Name { get; set; }
        public Array noticelist { get; set; }
        public string flag { get; set; }
        public long stud_count { get; set; }
        public string EME_ExamName { get; set; }
        public string EME_ExamCode { get; set; }
        public string messag { get; set; }
        public Array yearlist1 { get; set; }
        public int EME_ExamOrder { get; set; }
        public string NameOfDesig { get; set; }
        public long absentee { get; set; }
        public decimal ballance { get; set; }
        public decimal recived { get; set; }
        public decimal paid { get; set; }
        public Array coedata { get; set; }
        public string eventName { get; set; }
        public string eventDesc { get; set; }
        public DateTime? COEE_EStartDate { get; set; }
        public DateTime? COEE_EEndDate { get; set; }
        public Array CurrentAcademicYear { get; set; }

        public Array notification { get; set; }
        public Array leavenotification { get; set; }
        public Array staffbrthlist { get; set; }
        public Array employeedetails { get; set; }
        
        public int ASMCL_Order { get; set; }
        public long HRME_Id { get; set; }
        public string employeeName { get; set; }

        public string OnClickOrOnChange { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public Array mobile { get; set; }
        public Array email { get; set; }


    }
}


