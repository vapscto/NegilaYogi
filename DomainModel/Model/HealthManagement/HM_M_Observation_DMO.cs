using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.HealthManagement
{
    [Table("HM_M_Observation", Schema = "HM")]
    public class HM_M_Observation_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HMMOBS_Id { get; set; }
        public long MI_Id { get; set; }
        public string HMMOBS_Observation { get; set; }
        public string HMMOBS_ObservationDesc { get; set; }
        public bool HMMOBS_ActiveFlg { get; set; }
        public DateTime? HMMOBS_CreatedDate { get; set; }
        public DateTime? HMMOBS_UpdatedDate { get; set; }
        public long HMMOBS_CreatedBy { get; set; }
        public long HMMOBS_UpdatedBy { get; set; }
    }
}
