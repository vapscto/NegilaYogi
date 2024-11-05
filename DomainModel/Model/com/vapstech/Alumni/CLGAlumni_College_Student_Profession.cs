using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Alumni
{
    [Table("Alumni_College_Student_Profession", Schema = "CLG")]
    public class CLGAlumni_College_Student_Profession 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long ALCSPR_Id { get; set; }
        public long MI_Id { get; set; }
        public long ALCMST_Id { get; set; }
        public string ALCSPR_CompanyName { get; set; }
        public string ALCSPR_CompanyAddress { get; set; }

        public string ALCSPR_Designation { get; set; }
        public string ALCSPR_EmailId { get; set; }

        public long? ALCSPR_Phone { get; set; }
        public string ALCSPR_WorkingSince { get; set; }

        public string ALCSPR_OtherDetails { get; set; }
        public Boolean ALCSPR_ActiveFlg { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }


    }
}


