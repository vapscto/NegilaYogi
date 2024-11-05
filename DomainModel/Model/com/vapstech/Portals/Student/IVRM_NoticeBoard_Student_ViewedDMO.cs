using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Portals.Student
{
    [Table("IVRM_NoticeBoard_Student_Viewed")]
    public class IVRM_NoticeBoard_Student_ViewedDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INTBCSTDV_Id { get; set; }
        public long INTB_Id { get; set; }
        public long AMST_Id { get; set; }

        public bool INTBCSTDV_ActiveFlag { get; set; }
        public DateTime INTBCSTDV_CreatedDate { get; set; }
        public DateTime INTBCSTDV_UpdatedDate { get; set; }

        public long INTBCSTDV_CreatedBy { get; set; }
        public long INTBCSTDV_UpdatedBy { get; set; }


    }
}
