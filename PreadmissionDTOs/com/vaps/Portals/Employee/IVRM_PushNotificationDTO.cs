using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Portals.Employee
{
   public class IVRM_PushNotificationDTO:CommonParamDTO
    {

        public Master_NumberingDTO transnumbconfigurationsettingsss { get; set; }
        public string trans_id { get; set; }
        public long IPN_Id { get; set; }
        public long MI_Id { get; set; }
        public string IPN_No { get; set; }
        public string IPN_StuStaffFlg { get; set; }
        public long ASMAY_Id { get; set; }
        public DateTime IPN_Date { get; set; }
        public string IPN_PushNotification { get; set; }
        public long IVRMUL_Id { get; set; }
        public bool IPN_ActiveFlag { get; set; }


        public long IPNST_Id { get; set; }
        public long HRME_Id { get; set; }
        public bool IPNST_ActiveFlag { get; set; }
        public long IVRMRT_Id { get; set; }

        public long IPNS_Id { get; set; }
        public long AMST_Id { get; set; }
        public bool IPNS_ActiveFlag { get; set; }
        public long UserId { get; set; }



        public long ASMCL_Id { get; set; }
        public string AMST_FirstName { get; set; }
        public string AMST_MiddleName { get; set; }
        public string AMST_LastName { get; set; }
        public string AMST_AdmNo { get; set; }
        public long ASMS_Id { get; set; }
        public string type { get; set; }
        public string flag { get; set; }
        public string flag_Type { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string HRME_EmployeeMiddleName { get; set; }
        public string HRME_EmployeeLastName { get; set; }
        public string roletype { get; set; }
        public bool duplicate { get; set; }
        public long IVRMR_Id { get; set; }

        public bool returnval { get; set; }
        public Array studentlist { get; set; }
        public Array stafflist { get; set; }
        public Array studentdata { get; set; }
        public Array empdata { get; set; }
        public Array editstaflist { get; set; }
        public Array editstudlist { get; set; }
        public Array modalstudlist { get; set; }
        public string ASMCL_ClassName { get; set; }

        public Array deviceArray { get; set; }
        
        public IVRM_PushNotificationDTO[] stud_ids { get; set; }

    }
}
