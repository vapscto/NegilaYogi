using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.HealthManagement
{
    [Table("HM_T_Illness", Schema = "HM")]
    public class HM_T_IllnessDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HMTILL_Id { get; set; }
        public long HMMILL_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public DateTime? HMTILL_Date { get; set; }
        public bool HMTILL_ActiveFlg { get; set; }
        public DateTime? HMTILL_CreatedDate { get; set; }
        public DateTime? HMTILL_UpdatedDate { get; set; }
        public long HMTILL_CreatedBy { get; set; }
        public long HMTILL_UpdatedBy { get; set; }
    }
}
