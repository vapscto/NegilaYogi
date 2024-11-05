using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Admission
{
    [Table("Adm_College_Master_Section", Schema = "CLG")]
    public class Adm_College_Master_SectionDMO:CommonParamDMO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ACMS_Id { get; set; }
        public long MI_Id { get; set; }
        public string ACMS_SectionName { get; set; }
        public string ACMS_SectionCode { get; set; }
        public int ACMS_Order { get; set; }
        public bool ACMS_ActiveFlag { get; set; }
        public int ACMS_MaxCapacity { get; set; }
    }
}
