using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.COE
{
    [Table("COE_Events_SMSMailPN", Schema = "COE")]
   public class COE_Events_SMSMailPN_DMO
    {
        [Key]
         [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long COEESMSMPN_Id { get; set; }
        public int COEE_Id { get; set; }
        public bool COEESMSMPNSMS_Flg { get; set; }
        public DateTime? COEESMSMPNSMS_Date { get; set; }
        public bool COEESMSMPNMail_Flg { get; set; }
        public DateTime? COEESMSMPNMail_Date { get; set; }
        public bool? COEESMSMPNPN_Flg { get; set; }
        public DateTime? COEESMSMPNPN_Date { get; set; }


    }
}
