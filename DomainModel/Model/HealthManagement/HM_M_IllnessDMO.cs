using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.HealthManagement
{

    [Table("HM_M_Illness", Schema = "HM")]
    public class HM_M_IllnessDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HMMILL_Id { get; set; }
        public long MI_Id { get; set; }
        public string HMMILL_IllnessName { get; set; }
        public string HMMILL_IllnessDesc { get; set; }
        public bool HMMILL_ActiveFlg { get; set; }
        public DateTime? HMMILL_CreatedDate { get; set; }
        public DateTime? HMMILL_UpdatedDate { get; set; }
        public long HMMILL_CreatedBy { get; set; }
        public long HMMILL_UpdatedBy { get; set; }
    }
}
