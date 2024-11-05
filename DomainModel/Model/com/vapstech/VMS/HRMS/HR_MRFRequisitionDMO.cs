using DomainModel.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.VMS.HRMS
{
    [Table("HR_MRFRequisition")]
    public class HR_MRFRequisitionDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMRFR_Id { get; set; }
        public long HRMP_Id { get; set; }
        public long HRMC_Id { get; set; }
        public long HRMD_Id { get; set; }
        public long HRMPR_Id { get; set; }
        public long HRMPT_Id { get; set; }
        public long IVRMMG_Id { get; set; }
        public long HRMRFR_MRFNO { get; set; }
        public long HRMRFR_NoofPosition { get; set; }
        public string HRMRFR_Skills { get; set; }
        public string HRMRFR_JobDesc { get; set; }
        public decimal HRMRFR_ExpFrom { get; set; }
        public decimal HRMRFR_ExpTo { get; set; }
        public long HRMRFR_Age { get; set; }
        public string HRMRFR_Reason { get; set; }
        public string HRMRFR_Remark { get; set; }
        public bool HRMRFR_WrittenTestFlg { get; set; }
        public bool HRMRFR_OnlineTestFlg { get; set; }
        public bool HRMRFR_TechnicalInterviewFlg { get; set; }
        public string HRMRFR_Attachment { get; set; }
        public bool HRMRFR_ActiveFlag { get; set; }
        public string HRMRFR_Status { get; set; }
        public long MI_Id { get; set; }
        public long HRMRFR_CreatedBy { get; set; }
        public long HRMRFR_UpdatedBy { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }
        public bool HRMRFR_ManagerFlag { get; set; }
        public bool? HRMRFR_HRFlag { get; set; }
        public bool? HRMRFR_MDFlag { get; set; }
        public decimal? HRMRFR_PayScaleFrom { get; set; }
        public decimal? HRMRFR_PayScaleTo { get; set; }
        public DateTime? HRMRFR_PositionFilled { get; set; }
        public string HRMRFR_HRComment { get; set; }
        public string HRMRFR_MDComment { get; set; }
        public string HRMRFR_JobLocation { get; set; }
    }

}