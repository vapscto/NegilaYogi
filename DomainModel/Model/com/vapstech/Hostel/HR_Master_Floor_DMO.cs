using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.Hostel
{
    [Table("HL_Master_Floor")]
    public class HR_Master_Floor_DMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HLMF_Id { get; set; }
        public long MI_Id { get; set; }
        public long HLMH_Id { get; set; }
        public string HRMF_FloorName { get; set; }
        public long HRMF_TotalRooms { get; set; }
        public string HRMF_FloorDesc { get; set; }
        public bool HRMF_ActiveFlag { get; set; }
        public long HRMF_CreatedBy { get; set; }
        public long HRMF_UpdatedBy { get; set; }

        public List<HL_Master_Floor_Facilities_DMO> HL_Master_Floor_Facilities_DMO { get; set; }

    }
}