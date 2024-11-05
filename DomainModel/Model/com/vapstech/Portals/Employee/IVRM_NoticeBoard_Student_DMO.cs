using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.Portals.Employee
{
    [Table("IVRM_NoticeBoard_Student")]
    public class IVRM_NoticeBoard_Student_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INTBCSTD_Id { get; set; }
        public long INTB_Id { get; set; }
        public long AMST_Id { get; set; }
        public bool INTBCSTD_ActiveFlag { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long INTBCSTD_CreatedBy { get; set; }
        public long INTBCSTD_UpdatedBy { get; set; }
    }
}
