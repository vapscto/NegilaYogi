using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.FrontOffice
{
    [Table("Adm_Student_Punch_College", Schema = "CLG")]
    public  class Adm_Student_Punch_CollegeDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long ASPUC_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMCST_Id { get; set; }
        public DateTime? ASPUC_PunchDate { get; set; }
        public bool ASPUC_ActiveFlg { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool ASPUC_ManualEntryFlg { get; set; }
        public long? ASPUC_CreatedBy { get; set; }
        public long? ASPUC_UpdatedBy { get; set; }


    }
}
