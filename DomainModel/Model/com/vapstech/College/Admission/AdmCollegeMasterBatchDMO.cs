using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Admission
{
    [Table("Adm_College_Master_Batch",Schema ="CLG")]
    public class AdmCollegeMasterBatchDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ACMB_Id { get; set; }
        public long MI_Id { get; set; }
        public string ACMSN_SessionName { get; set; }
        public int ACMNS_Order { get; set; }
        public bool ACMSN_ActiveFlag { get; set; }
    }
}
