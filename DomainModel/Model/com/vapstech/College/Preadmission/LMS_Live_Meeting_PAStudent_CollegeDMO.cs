using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.College.Preadmission
{
    [Table("LMS_Live_Meeting_PAStudent_College", Schema = "CLG")]
    public class LMS_Live_Meeting_PAStudent_CollegeDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LMSLMEETPASTDC_Id { get; set; }
        public long LMSLMEET_Id { get; set; }
        public long PACA_Id { get; set; }
        public string LMSLMEETPASTDC_LoginTime { get; set; }
        public string LMSLMEETPASTDC_MACAddress { get; set; }
        public string LMSLMEETPASTDC_IPAddress { get; set; }
        public string LMSLMEETPASTDC_LogoutTime { get; set; }
        public bool LMSLMEETPASTDC_ActiveFlg { get; set; }
        public DateTime? LMSLMEETPASTDC_CreatedDate { get; set; }
        public DateTime? LMSLMEETPASTDC_UpdatedDate { get; set; }
        public long LMSLMEETPASTDC_CreatedBy { get; set; }
        public long LMSLMEETPASTDC_UpdatedBy { get; set; }
    }
}
