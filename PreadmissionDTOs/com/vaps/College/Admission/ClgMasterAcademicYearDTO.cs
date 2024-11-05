using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.College.Admission
{
    public class ClgMasterAcademicYearDTO
    {
        public long ACMAY_Id { get; set; }
        public long MI_Id { get; set; }
        public string ACMAY_AcademicYear { get; set; }
        public string ACMAY_AcademicYearCode { get; set; }
        public DateTime? ACMAY_AYFromDate { get; set; }
        public DateTime? ACMAY_AYToDate { get; set; }
        public int ACMAY_AYOrder { get; set; }
        public int ACMAY_AYBatchOrder { get; set; }
        public bool ACMAB_PAActiveFlg { get; set; }
        public Array getdetails { get; set; }
        public Array getyear { get; set; }
        public string message { get; set; }
        public bool returnval { get; set; }
        public Array editdetails { get; set; }
        public yearorder_details[] yearorder { get; set; }
    }

    public class yearorder_details
    {
        public long ACMAY_Id { get; set; }
        public int ACMAY_AYOrder { get; set; }
    }
}
