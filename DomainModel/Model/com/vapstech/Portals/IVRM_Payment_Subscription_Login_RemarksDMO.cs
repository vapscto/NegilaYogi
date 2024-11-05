using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Portals
{
    [Table("IVRM_Payment_Subscription_Login_Remarks")]
    public class IVRM_Payment_Subscription_Login_RemarksDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IVRMPSLR_Id { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public string IVRMPSLR_Remarks { get; set; }
        public string IVRMPSLR_RemainderTemplateName { get; set; }
        public DateTime? IVRMPSLR_LoginDatetime { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
