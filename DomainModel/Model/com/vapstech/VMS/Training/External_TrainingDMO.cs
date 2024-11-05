using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.Training
{
    [Table("HR_External_Training")]
    public class External_TrainingDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HREXTTRN_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public long HRMETRCEN_Id { get; set; }
        public long HRMETRTY_Id { get; set; }
        public string HREXTTRN_TrainingTopic { get; set; }
        public DateTime? HREXTTRN_StartDate { get; set; }
        public DateTime? HREXTTRN_EndDate { get; set; }
        public string HREXTTRN_StartTime { get; set; }
        public string HREXTTRN_EndTime { get; set; }
        public decimal HREXTTRN_TotalHrs { get; set; }
        public string HREXTTRN_CertificateFileName { get; set; }
        public string HREXTTRN_CertificateFilePath { get; set; }
        public string HREXTTRN_ApprovedFlg { get; set; }
        public bool HREXTTRN_ActiveFlag { get; set; }
        public DateTime HREXTTRN_CreatedDate { get; set; }
        public DateTime HREXTTRN_UpdatedDate { get; set; }
        public long HREXTTRN_CreatedBy { get; set; }
        public long HREXTTRN_UpdatedBy { get; set; }
    }
}
