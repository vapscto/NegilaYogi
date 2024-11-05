using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.HRMS
{
    public class HR_Employee_StudentActivitiesDTO
    {
        public long HRESACT_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public string HRESACT_TypeOfActivity { get; set; }
        public DateTime HRESACT_ActivityDate { get; set; }
        public string HRESACT_OrgAgency { get; set; }
        public string HRESACT_Place { get; set; }
        public string HRESACT_Duration { get; set; }
        public string HRESACT_Role { get; set; }
        public string HRESACT_Document { get; set; }
        public bool HRESACT_ActiveFlg { get; set; }
        public long HRESACT_CreatedBy { get; set; }
        public long HRESACT_UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; } 
        public string retrunMsg { get; set; }
        public string HRESACT_Year { get; set; }
    }
}
