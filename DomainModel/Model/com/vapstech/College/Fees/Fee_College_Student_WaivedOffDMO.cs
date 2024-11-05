using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.College.Fees
{
    [Table("Fee_College_Student_WaivedOff", Schema = "CLG")]
    public class Fee_College_Student_WaivedOffDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FCSWO_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long FCMAS_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public DateTime FCSWO_Date { get; set; }
        public long FMH_Id { get; set; }
        public long FMG_Id { get; set; }
        public long FTI_Id { get; set; }
        public long FCSWO_WaivedOffAmount { get; set; }
        public bool FCSWO_ActiveFlag { get; set; }
        public long User_Id { get; set; }
    }
}
