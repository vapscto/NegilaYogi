using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.admission
{
    [Table("IVRM_SMS_Mail_Header")]
    public class SMSMail_HeaderDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMH_Id { get; set; }
       public string ISMH_HeaderName { get; set; }
    }
}
