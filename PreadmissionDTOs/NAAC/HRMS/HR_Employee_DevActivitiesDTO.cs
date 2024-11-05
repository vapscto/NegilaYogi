using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.HRMS
{
    public class HR_Employee_DevActivitiesDTO
    {
        public long HREDACT_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public string HREDACT_DevelopmentActivity { get; set; }
        public DateTime HREDACT_ActivityDate { get; set; }
        public string HREDACT_OrgAgency { get; set; }
        public string HREDACT_Place { get; set; }
        public string HREDACT_Duration { get; set; }
        public string HREDACT_Role { get; set; }
        public string HREDACT_Document { get; set; }
        public bool HREDACT_ActiveFlg { get; set; }
        public long HREDACT_CreatedBy { get; set; }
        public long HREDACT_UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string retrunMsg { get; set; }
        public string HREDACT_Year { get; set; }
        public string HREDACT_ActivityLevel { get; set; }
    }
}
