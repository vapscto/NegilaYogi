using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.Training
{
  public  class Master_External_TrainingCentersDTO
    {

        public long HRMETRCEN_Id { get; set; }
        public long MI_Id { get; set; }
        public long Userid { get; set; }
        public long roleid { get; set; }
        public string HRMETRCEN_TrainingCenterName { get; set; }
        public string HRMETRCEN_ContactPersonName { get; set; }
        public string HRMETRCEN_ContactNo { get; set; }
        public string HRMETRCEN_ContactEmailId { get; set; }
        public string HRMETRCEN_CenterAddress { get; set; }
        public bool HRMETRCEN_ActiveFlag { get; set; }
        public DateTime HRMETRCEN_CreatedDate { get; set; }
        public DateTime HRMETRCEN_UpdatedDate { get; set; }
        public long HRMETRCEN_CreatedBy { get; set; }
        public long HRMETRCEN_UpdatedBy { get; set; }
        public bool returnval { get; set; }
        public string message { get; set; }
        public Array getloaddetails { get; set; }
    }
}
