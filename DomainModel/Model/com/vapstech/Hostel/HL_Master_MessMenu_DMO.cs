using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Hostel
{
    [Table("HL_Master_MessMenu")]
    public class HL_Master_MessMenu_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HLMMN_Id { get; set; }
        public long MI_Id { get; set; }
        public long HLMM_Id { get; set; }
        public long HLMMC_Id { get; set; }
        public string HLMMN_MenuName { get; set; }
        public string HLMMN_MenuDesc { get; set; }
        public bool HLMMN_ActiveFlag { get; set; }
        public DateTime? HLMMN_CreatedDate { get; set; }
        public DateTime? HLMMN_UpdatedDate { get; set; }
        public long HLMMN_CreatedBy { get; set; }
        public long HLMMN_UpdatedBy { get; set; }
    }
}
