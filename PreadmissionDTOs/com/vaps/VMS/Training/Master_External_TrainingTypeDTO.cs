using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.Training
{
    public class Master_External_TrainingTypeDTO
    {
        public long MI_Id { get; set; }
        public long Userid { get; set; }
        public long roleid { get; set; }
        public long HRMETRTY_Id { get; set; }
        public Array getloaddetails { get; set; }
        public string HRMETRTY_ExternalTrainingType { get; set; }
        public decimal HRMETRTY_MinimumTrainingHrs { get; set; }
        public bool HRMETRTY_ActiveFlag { get; set; }
        public DateTime HRMETRTY_CreatedDate { get; set; }
        public DateTime HRMETRTY_UpdatedDate { get; set; }
        public long HRMETRTY_CreatedBy { get; set; }
        public long HRMETRTY_UpdatedBy { get; set; }
        public bool returnval { get; set; }
        public string message { get; set; }
    }
}
