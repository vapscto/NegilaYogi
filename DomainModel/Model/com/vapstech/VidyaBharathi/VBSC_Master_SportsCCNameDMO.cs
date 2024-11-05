using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VidyaBharathi
{
    [Table("VBSC_Master_SportsCCName")]
    public class VBSC_Master_SportsCCNameDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long VBSCMSCC_Id { get; set; }
        public long VBSCMSCCG_Id { get; set; }
        public long MT_Id { get; set; }
        public string VBSCMSCC_SportsCCName { get; set; } 
        public string VBSCMSCC_SportsCCDesc { get; set; }
        public string VBSCMSCC_SGFlag { get; set; }
        public long VBSCMSCC_NoOfMembers { get; set; }
        public string VBSCMSCC_RecHighLowFlag { get; set; }
        public string VBSCMSCC_RecInfo { get; set; }
        public string VBSCMSCC_GenderFlg { get; set; }
        public bool VBSCMSCC_ActiveFlag { get; set; }
        public DateTime? VBSCMSCC_CreatedDate { get; set; }
        public DateTime? VBSCMSCC_UpdatedDate { get; set; }

    }
}
