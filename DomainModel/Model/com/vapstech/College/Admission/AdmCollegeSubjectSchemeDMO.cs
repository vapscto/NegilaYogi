using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Admission
{
    [Table("Adm_College_SubjectScheme", Schema = "CLG")]
    public class AdmCollegeSubjectSchemeDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ACSS_Id { get; set; }
        public long MI_Id { get; set; }
        public string ACSS_SchmeName { get; set; }
        public bool ACST_ActiveFlg { get; set; }

    }
}
