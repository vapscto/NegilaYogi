using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.Hostel
{
    [Table("HL_Master_Room")]
    public class HR_Master_Room_DMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMRM_Id { get; set; }
        public long MI_Id { get; set; }
        public long HLMH_Id { get; set; }
        public long HLMF_Id { get; set; }
        public string HRMRM_RoomNo { get; set; }
        public string HRMRM_SharingFlg { get; set; }
        public string HRMRM_ACFlg { get; set; }
        public long HRMRM_BedCapacity { get; set; }
        public string HRMRM_RoomDesc { get; set; }
        public bool HRMRM_RoomForStudentFlg { get; set; }
        public bool HRMRM_RoomForStaffFlg { get; set; }
        public bool HRMRM_RoomForGuestFlg { get; set; }
        public bool HRMRM_ActiveFlag { get; set; }
        public long HRMRM_CreatedBy { get; set; }
        public long HRMRM_UpdatedBy { get; set; }
        public long HLMRCA_Id { get; set; }

        public List<HL_Master_Room_Facilities_DMO> HL_Master_Room_Facilities_DMO { get; set; }

    }
}
