using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.VMS.Training
{
    [Table("HR_Training_Create")]
    public class HR_Training_Create_DMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRTCR_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRTCR_PrgogramName { get; set; }
        public long HRMD_Id { get; set; }
        public string HRTCR_ProgramDesc { get; set; }
        public bool HRTCR_CostFeeFlg { get; set; }
        public decimal HRTCR_Cost { get; set; }
        public long HRMB_Id { get; set; }
        public long HRMF_Id { get; set; }
        public long HRMR_Id { get; set; }
        public bool HRTCR_InternalORExternalFlg { get; set; }
        public bool HRTCR_ActiveFlag { get; set; }
        public long HRTCR_StatusFlg { get; set; }
        public long HRTCR_CreatedBy { get; set; }
        public long HRTCR_UpdatedBy { get; set; }
        public DateTime? HRTCR_StartDate { get; set; }
        public DateTime? HRTCR_EndDate { get; set; }
    
        public List<HR_Training_Create_Participants_DMO> HR_Training_Create_Participants_DMO { get; set; }

    }
}