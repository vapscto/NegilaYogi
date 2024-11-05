using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Hostel
{
    [Table("HL_Master_Mess")]
    public class HL_Master_Mess_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HLMM_Id { get; set; }
        public long MI_Id { get; set; }
        public string HLMM_Name { get; set; }
     //   public long HLMMC_Id { get; set; }
        public bool HLMM_VegFlg { get; set; }
        public bool HLMM_NonVegFlg { get; set; }
        public string HLMM_BFSStartTime { get; set; }
        public string HLMM_BFSEndTime { get; set; }
        public string HLMM_LNStartTime { get; set; }
        public string HLMM_LNEndTime { get; set; }
        public string HLMM_LNTSStartTime { get; set; }
        public string HLMM_LNTSEndTime { get; set; }
        public string HLMM_DNSStartTime { get; set; }
        public string HLMM_DNSEndTime { get; set; }
        public bool HLMM_ActiveFlag { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long? HLMM_CreatedBy { get; set; }
        public long? HLMM_UpdatedBy { get; set; }

    }
}
