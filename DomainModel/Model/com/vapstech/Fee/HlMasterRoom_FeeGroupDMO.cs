using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Fee
{
    [Table("HL_Master_Room_FeeGroup")]
    public class HlMasterRoom_FeeGroupDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HLMRFG_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRMRM_Id { get; set; }
        public long FMG_Id { get; set; }
        public bool HLMRFG_ActiveFlag { get; set; }
        public DateTime? HLMRFG_CreatedDate { get; set; }
        public long? HLMRFG_CreatedBy { get; set; }
        public DateTime? HLMRFG_UpdatedDate { get; set; }
        public long? HLMRFG_UpdatedBy { get; set; }
    }
}
