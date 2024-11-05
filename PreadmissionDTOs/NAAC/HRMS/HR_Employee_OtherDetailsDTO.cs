using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.HRMS
{
    public class HR_Employee_OtherDetailsDTO
    {
        public long HREOTHDET_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public string HREOTHDET_OtherDetails { get; set; }
        public string HREOTHDET_Document { get; set; }
        public bool HREOTHDET_ActiveFlg { get; set; }
        public long HREOTHDET_CreatedBy { get; set; }
        public long HREOTHDET_UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public String retrunMsg { get; set; }
        public string HREOTHDET_Year { get; set; }
    }
}
