using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.HRMS
{
    public class HR_Employee_OrientationCourseDTO
    {
        public long HREORCO_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public string HREORCO_OrientationCourse { get; set; }
        public string HREORCO_Place { get; set; }
        public string HREORCO_Duration { get; set; }
        public DateTime HREORCO_From { get; set; }
        public DateTime HREORCO_To { get; set; }
        public string HREORCO_SposoringAuthority { get; set; }
        public string HREORCO_Remarks { get; set; }
        public string HREORCO_Document { get; set; }
        public bool HREORCO_ActiveFlg { get; set; }
        public long HREORCO_CreatedBy { get; set; }
        public long HREORCO_UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string retrunMsg { get; set; }
        public string HREORCO_Year { get; set; }
        public string HREORCO_Title { get; set; }
    }
}
