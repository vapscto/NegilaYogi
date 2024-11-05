using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Hostel
{
    [Table("HL_Master_Hostel_Photos")]
    public class HL_Master_Hostel_Photos_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HLMHP_Id { get; set; }
        public long MI_Id { get; set; }
        public long HLMH_Id { get; set; }
        public string HLMHP_FileName { get; set; }
        public string HLMHP_FilePath { get; set; }
        public bool HLMHP_ActiveFlg { get; set; }
    }
}
