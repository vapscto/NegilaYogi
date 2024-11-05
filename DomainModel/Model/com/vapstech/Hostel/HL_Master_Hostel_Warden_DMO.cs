using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Hostel
{
    [Table("HL_Master_Hostel_Warden")]
    public class HL_Master_Hostel_Warden_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HLMHW_Id { get; set; }
        public long MI_Id { get; set; }
        public long HLMH_Id { get; set; }
        public long HRME_Id { get; set; }
        public string HLMHW_WardenType { get; set; }
        public bool HLMHW_ActiveFlg { get; set; }

    }
}
