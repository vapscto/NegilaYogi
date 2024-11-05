using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission
{
    public class MasterCycleYearMappingDTO
    {
        public long NCMACY_Id { get; set; }
        public long MI_Id { get; set; }
        public string NCMACY_NAACCycle { get; set; }
        public DateTime? NCMACY_FromDate { get; set; }
        public DateTime? NCMACY_TODate { get; set; }
        public bool NCMACY_ActiveFlg { get; set; }
        public long NCMACY_CreatedBy { get; set; }
        public long NCMACY_UpdatedBy { get; set; }
        public int NCMACY_Order { get; set; }
        public DateTime? NCMACY_CreatedDate { get; set; }
        public DateTime? NCMACY_UpdatedDate { get; set; }
        public Array getmastercycle { get; set; }
        public Array getmastercycleorder { get; set; }
        public Array getmastercyclemapping { get; set; }
        public Array geteditdetails { get; set; }
        public Array getyearlist { get; set; }
        public Array getmastercyclemappingdetails { get; set; }
        public Array getviewdetails { get; set; }
        public bool returnval { get; set; }
        public string message { get; set; }
        public string ASMAY_Year { get; set; }
        public long NCMACYYR_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public int ASMAY_Order { get; set; }
        public bool NCMACYYR_ActiveFlg { get; set; }
        public temp_yeardto[] temp_yeardto { get; set; }
        public order_temp[] order_temp { get; set; }
    }
    public class temp_yeardto
    {
        public long ASMAY_Id { get; set; }
    }
    public class order_temp
    {
        public int NCMACY_Order { get; set; }
        public long NCMACY_Id { get; set; }
    }
}
