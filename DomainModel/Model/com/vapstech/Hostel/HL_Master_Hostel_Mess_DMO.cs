using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Hostel
{
    [Table("HL_Master_Hostel_Mess")]
    public class HL_Master_Hostel_Mess_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HLMHMS_Id { get; set; }
        public long MI_Id { get; set; }
        public long HLMH_Id { get; set; }
        public long HLMM_Id { get; set; }
        public bool HLMHMS_ActiveFlg { get; set; }
    }
}
