using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.Training
{
    [Table("HR_Master_External_TrainingType")]
    public class Master_External_TrainingTypeDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMETRTY_Id { get; set; }
        public long MI_Id { get; set; }       
        public string HRMETRTY_ExternalTrainingType { get; set; }
        public decimal HRMETRTY_MinimumTrainingHrs { get; set; }
        public bool HRMETRTY_ActiveFlag { get; set; }
        public DateTime HRMETRTY_CreatedDate { get; set; }
        public DateTime HRMETRTY_UpdatedDate { get; set; }
        public long HRMETRTY_CreatedBy { get; set; }
        public long HRMETRTY_UpdatedBy { get; set; }
    }
}
