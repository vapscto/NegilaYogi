using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.VMS.Training
{
    [Table("HR_Master_Room")]
    public class HR_Master_Room : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMR_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRMR_RoomName { get; set; }
        public long HRMB_Id { get; set; }
        public long HRMF_Id { get; set; }
        public bool HRMR_ActiveFlag { get; set; }
        public long HRMR_CreatedBy { get; set; }
        public long HRMR_UpdatedBy { get; set; }

        public bool  HRMR_OutSideBookingFlg { get; set; }
        public long HRMR_Capacity { get; set; }
        public decimal HRMR_RentPerDay { get; set; }
        public long HRMR_NoOfHrs { get; set; }
        public decimal HRMR_RentPerHour { get; set; }
        public string HRMR_TypeFlag { get; set; }
        public string HRMR_Desc { get; set; }
        public List<HR_Master_Room_ContactsDMO> HR_Master_Room_ContactsDMO { get; set; }
        public List<HR_Master_Room_AmenitiesDMO> HR_Master_Room_AmenitiesDMO { get; set; }
        public List<HR_Master_Room_FilesDMO> HR_Master_Room_FilesDMO { get; set; }

    }
}
