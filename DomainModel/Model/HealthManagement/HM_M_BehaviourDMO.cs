using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.HealthManagement
{
    [Table("HM_M_Behaviour", Schema = "HM")]
    public class HM_M_BehaviourDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HMMBEH_Id { get; set; }
        public long MI_Id { get; set; }
        public string HMMBEH_BehaviourName { get; set; }
        public string HMMBEH_BehaviourDesc { get; set; }
        public bool HMMBEH_ActiveFlg { get; set; }
        public DateTime? HMMBEH_CreatedDate { get; set; }
        public DateTime? HMMBEH_UpdatedDate { get; set; }
        public long HMMBEH_CreatedBy { get; set; }
        public long HMMBEH_UpdatedBy { get; set; }
    }
}
