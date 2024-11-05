
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;

namespace DomainModel.Model.com.vapstech.College.Portals.IVRM
{
    [Table("IVRM_NoticeBoard_Student_College", Schema = "CLG")]
    public class IVRM_NoticeBoard_Student_CollegeDMO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INTBCSTDC_Id { get; set; }
        public long INTB_Id { get; set; }
        public long AMCST_Id { get; set; }
        public bool INTBCSTDC_ActiveFlag { get; set; }
        public DateTime INTBCSTDC_CreatedDate { get; set; }
        public DateTime INTBCSTDC_UpdatedDate { get; set; }
        public long INTBCSTDC_CreatedBy { get; set; }
        public long INTBCSTDC_UpdatedBy { get; set; }
       
    }
}