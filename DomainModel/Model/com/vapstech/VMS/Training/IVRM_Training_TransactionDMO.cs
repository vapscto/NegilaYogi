using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.Training
{
    [Table("IVRM_Training_Transaction")]
    public class IVRM_Training_TransactionDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IVRMTT_Id { get; set; }
        public string   IVRMTT_EmployeeName { get; set; }
        public long IVRMTT_EmployeeId  { get; set; }
        public string IVRMTT_EmployeeRole { get; set; }
        public string IVRMTT_EmployeeEmail { get; set; }
        public string IVRMTT_EmployeePhone { get; set; }
        public string IVRMTT_ClientName { get; set; }
        public string IVRMTT_ClientURL { get; set; }
        public long IVRMTT_ClientMI_Id { get; set; }
        public DateTime? IVRMTT_TentetiveDate { get; set; }
        public string IVRMTT_TentetiveStartTime { get; set; }
        public string IVRMTT_TentetiveEndTime { get; set; }
        public string IVRMTT_TrainingMode { get; set; }
        public string IVRMTT_Remarks { get; set; }
        public string IVRMTT_Status { get; set; }
        public DateTime? IVRMTT_TrainingDate { get; set; }
        public string IVRMTT_TrainingStartTime { get; set; }
        public string IVRMTT_TrainingToTime { get; set; }
        public string IVRMTT_TrainerRemarks { get; set; }
        public decimal? IVRMTT_Duration { get; set; }
        public string IVRMTT_Feedback { get; set; }
        public bool? IVRMTT_ActiveFlag { get; set; }
        public DateTime? IVRMTT_CreatedDate { get; set; }
        public DateTime? IVRMTT_UpdatedDate { get; set; }
        public long? IVRMTT_CreatedBy { get; set; }
        public long? IVRMTT_UpdatedBy { get; set; }
    }
}
