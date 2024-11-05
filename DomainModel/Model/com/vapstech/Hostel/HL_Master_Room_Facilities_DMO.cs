using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Hostel
{
    [Table("HL_Master_Room_Facilities")]
    public class HL_Master_Room_Facilities_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMRMF_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRMRM_Id { get; set; }
        public long HLMFTY_Id { get; set; }
        public bool HRMRMF_ActiveFlg { get; set; }

    }
}
