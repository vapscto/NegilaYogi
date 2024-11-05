using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Portals.IVRM
{
    public class ClgPushNotificationDTO
    {
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public bool already_cnt { get; set; }
        public string message { get; set; }
        public long UserId { get; set; }
        public Array roletype { get; set; }

        public long AMCST_Id { get; set; }
        public long MI_Id { get; set; }    
        public long ASMAY_Id { get; set; }
        public string AMCST_FirstName { get; set; }
        public string AMCST_MiddleName { get; set; }
        public string AMCST_LastName { get; set; }    
        public string Role_flag { get; set; }     
        public long IVRMRT_Id { get; set; }
        public long HRME_Id { get; set; }
        public string trans_id { get; set; }

        public long IPN_Id { get; set; }   
        public string IPN_No { get; set; }
        public string IPN_StuStaffFlg { get; set; }       
        public DateTime IPN_Date { get; set; }
        public string IPN_PushNotification { get; set; }
        public long IVRMUL_Id { get; set; }
        public bool IPN_ActiveFlag { get; set; }

        public long ICPNS_Id { get; set; }  
        public bool ICPNS_ActiveFlag { get; set; }

        public Array namelist { get; set; }
        public Array notificationlist { get; set; }
        public Array notificationdetails { get; set; }

        
        public StudentArryDTO[] studentarray { get; set; }
        public Master_NumberingDTO transnumbconfigurationsettingsss { get; set; }
    }

    public class StudentArryDTO
    {
        public long AMCST_Id { get; set; }    
        public string AMCST_FirstName { get; set; }
      
    }
}
