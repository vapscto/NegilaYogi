using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.HealthManagement
{
    [Table("HM_M_Cleanness", Schema = "HM")]
    public class HM_M_CleannessDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HMMCLN_Id { get; set; }
        public long MI_Id { get; set; }
        public string HMMCLN_CleannessName { get; set; }
        public string HMMCLN_CleannessDesc { get; set; }
        public bool HMMCLN_ActiveFlg { get; set; }
        public DateTime? HMMCLN_CreatedDate { get; set; }
        public DateTime? HMMCLN_UpdatedDate { get; set; }
        public long HMMCLN_CreatedBy { get; set; }
        public long HMMCLN_UpdatedBy { get; set; }
    }
}
