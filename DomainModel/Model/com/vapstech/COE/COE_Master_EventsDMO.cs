using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.com.vapstech.COE
{
    //Table("Customers", Schema = "Ordering")]   
    [Table("COE_Master_Events", Schema = "COE")]
    public class COE_Master_EventsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int COEME_Id { get; set; }      
        public long MI_Id { get; set; }
        public string COEME_EventName { get; set; }
        public string COEME_EventDesc { get; set; }
        public string COEME_SMSMessage { get; set; }
        public string COEME_MailSubject { get; set; }
        // public string COEME_MailBody { get; set; }
        public string COEME_MailHeader { get; set; }
        public string COEME_MailFooter { get; set; }
        public string COEME_Mail_Message { get; set; }
        public string COEME_MailHTMLTemplate { get; set; }
        public bool COEME_ActiveFlag  { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }


    }
}
