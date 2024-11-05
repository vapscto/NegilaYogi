using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModel.Model.com.vapstech.College.Portals.IVRM;

namespace DomainModel.Model.com.vapstech.Portals.Employee
{
    [Table("IVRM_PushNotification")]
   public class IVRM_PushNotificationDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IPN_Id { get; set; }
        public long MI_Id { get; set; }
        public string IPN_No { get; set; }
        public string IPN_StuStaffFlg { get; set; }
        public long ASMAY_Id { get; set; }
        public DateTime IPN_Date { get; set; }
        public string IPN_PushNotification { get; set; }
        public long IVRMUL_Id { get; set; }
        public bool  IPN_ActiveFlag { get; set; }

        public List<IVRM_PushNotification_Staff_DMO> IVRM_PushNotification_Staff_DMO { get; set; }
        public List<IVRM_PushNotification_Student_DMO> IVRM_PushNotification_Student_DMO { get; set; }
        public List<IVRM_College_PN_StudentDMO> IVRM_College_PN_StudentDMO { get; set; }
        
    }
   
}
