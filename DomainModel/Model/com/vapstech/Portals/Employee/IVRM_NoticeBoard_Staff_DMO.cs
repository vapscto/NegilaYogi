using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.Portals.Employee
{
    [Table("IVRM_NoticeBoard_Staff")]
    public class IVRM_NoticeBoard_Staff_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INTBCSTF_Id { get; set; }
        public long INTB_Id { get; set; }
        public long HRME_Id { get; set; }
        public bool INTBCSTF_ActiveFlag { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long INTBCSTF_CreatedBy { get; set; }
        public long INTBCSTF_UpdatedBy { get; set; }
    }
}
