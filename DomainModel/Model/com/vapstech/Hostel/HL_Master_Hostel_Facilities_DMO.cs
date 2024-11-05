using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Hostel
{
    [Table("HL_Master_Hostel_Facilities")]
    public class HL_Master_Hostel_Facilities_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HLMHF_Id { get; set; }
        public long MI_Id { get; set; }
        public long HLMH_Id { get; set; }
        public long HLMFTY_Id { get; set; }
        public bool HLMHF_ActiveFlg { get; set; }

    }
}
