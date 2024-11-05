using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Hostel
{
    [Table("HL_Master_Bed")]
    public class HL_Master_Bed_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HLMB_Id { get; set; }
       public long MI_Id { get; set; }
       public long HLMH_Id { get; set; }
      // public long HRMF_Id { get; set; }
       public long HLMF_Id { get; set; }
        public long HRMRM_Id { get; set; }
      // public long HRMR_Id { get; set; }
       public string HLMB_BedName { get; set; }
       public bool HLMB_MattressFlg { get; set; }
       public bool HLMB_BedSheetFlg { get; set; }
       public bool HLMB_PillowFlg { get; set; }
       public bool HLMB_StudyTableFlg { get; set; }
       public bool HLMB_LampFlg { get; set; }
       public DateTime HLMB_CreatedDate { get; set; }
       public DateTime HLMB_UpdatedDate { get; set; }


    }
}
