using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Portals.Principal
{
    [Table("IVRM_NoticeBoard")]
    public class NoticeDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INTB_Id { get; set; }
        public long MI_Id { get; set; }
        public string INTB_Title { get; set; }
        public string INTB_Description { get; set; }
        public string INTB_Attachment { get; set; }
        public DateTime INTB_DisplayDate { get; set; }
        public bool INTB_ActiveFlag { get; set; }
        public DateTime INTB_StartDate{ get; set; }
        public DateTime INTB_EndDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
