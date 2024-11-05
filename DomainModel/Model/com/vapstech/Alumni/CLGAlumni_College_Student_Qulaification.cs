using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Alumni
{
    [Table("Alumni_College_Student_Qulaification", Schema = "CLG")]
    public class CLGAlumni_College_Student_Qulaification 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long ALCSQU_Id { get; set; }
        public long MI_Id { get; set; }
        public long ALCMST_Id { get; set; }
        public string ALCSQU_Qulification { get; set; }
        public long ALCSQU_YearOfPassing { get; set; }
        public string ALCSQU_University { get; set; }
        public string ALCSQU_OtherDetails { get; set; }
        public Boolean ALCSQU_ActiveFlg { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }


    }
}


