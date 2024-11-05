using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Alumni
{
    [Table("Alumni_College_Student_Achivement", Schema = "CLG")]
    public class CLGAlumni_College_Student_Achivement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ALCSAC_Id { get; set; }
        public long MI_Id { get; set; }
        public long ALCMST_Id { get; set; }
        public string ALCSAC_Achievement { get; set; }
        public string ALCSAC_Remarks { get; set; }
        public Boolean ALCSAC_ActiveFlg { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }


    }
}


