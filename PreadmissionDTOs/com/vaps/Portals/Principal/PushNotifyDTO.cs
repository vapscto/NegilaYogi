﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Portals.Principal
{
    public class PushNotifyDTO 
    {
        //public int EME_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long HRME_Id { get; set; }
        public long HRMDES_Id { get; set; }
        public string HRMDES_DesignationName { get; set; }
        public string HRME_EmployeeCode { get; set; }

        public Array employeedropdown { get; set; }
        public long?[] hrmgT_IdList { get; set; }
        public long?[] employeeTypeIdList { get; set; }
        public long?[] hrmD_IdList { get; set; }
        public long?[] hrmdeS_IdList { get; set; }
        public long ASMCL_Id { get; set; }
        public Array yearlist { get; set; }
        public Array classlist { get; set; }
        public Array sectionlist { get; set; }
        public string msg { get; set; }
        public string ASMCL_ClassName { get; set; }
        public long ASMS_Id { get; set; }
        public string ASMC_SectionName { get; set; }
        public Array studentlist { get; set; }
        public string Mobno { get; set; }
        public string mes { get; set; }
        public string HRME_EmployeeLastName { get; set; }
        public string HRME_EmployeeMiddleName { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string AMST_AdmNo { get; set; }
        public long AMST_MobileNo { get; set; }
        public string AMST_emailId { get; set; }
        public string AMST_AppDownloadedDeviceId { get; set; }
        public string HRME_AppDownloadedDeviceId { get; set; }
        public long AMST_Id { get; set; }
        public Array StaffName { get; set; }
        public long roleId { get; set; }
        public string selectedRadiobtn { get; set; }
        public Array currentYear { get; set; }
        public Array employe { get; set; }
        public Array stafflist { get; set; }
        public string studentName { get; set; }
        public int scnt { get; set; }
        public int fcnt { get; set; }
        public int studentCount { get; set; }
        public Array departmentdropdown { get; set; }
        public Array designationdropdown { get; set; }

        public string multipledep { get; set; }
        public string multipledes { get; set; }
        
        public PushNotifyDTO[] studentlistdto { get; set; }
        public string SmsMailText { get; set; }
        public string headername { get; set; }
        public string radiotype { get; set; }
        public string smsStatus { get; set; }
        public long? HRME_MobileNo { get; set; }
        public string emailStatus { get; set; }
        public string hrm_email { get; set; }

        
    }
}

