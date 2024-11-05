using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.College.Portals.IVRM
{
    [Table("IVRM_NoticeBoard_Student_College_Viewed")]
    public class IVRM_NoticeBoard_Student_College_ViewedDMO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INTBCSTDCV_Id { get; set; }
        public long INTB_Id { get; set; }
        public long ACMST_Id { get; set; }
        public bool INTBCSTDCV_ActiveFlag { get; set; }
        public DateTime INTBCSTDCV_CreatedDate { get; set; }
        public DateTime INTBCSTDCV_UpdatedDate { get; set; }
        public long INTBCSTDCV_CreatedBy { get; set; }
        public long INTBCSTDCV_UpdatedBy { get; set; }
    }
}
