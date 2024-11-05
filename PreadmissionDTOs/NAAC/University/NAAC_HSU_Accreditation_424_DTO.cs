using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.University
{
    public class NAAC_HSU_Accreditation_424_DTO
    {

        public long NCHSUA424_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCHSUA424_Year { get; set; }
        public bool NCHSUA424_NabhAcrFlag { get; set; }
        public bool NCHSUA424_NablAcrFlag { get; set; }
        public bool NCHSUA424_IntAcrFlag { get; set; }
        public bool NCHSUA424_ISOCertFlag { get; set; }
        public bool NCHSUA424_GplorGcplFlag { get; set; }
        public long NCHSUA424_CreatedBy { get; set; }
        public long NCHSUA424_UpdatedBy { get; set; }
        public DateTime NCHSUA424_CreatedDate { get; set; }
        public DateTime NCHSUA424_UpdatedDate { get; set; }
        public long UserId { get; set; }
        public Array institutionlist { get; set; }
        public Array allacademicyear { get; set; }
        public string ASMAY_Year { get; set; }
        public Array alldata1 { get; set; }
        public Array editlist { get; set; }
        public bool returnval { get; set; }
        public string flag { get; set; }
        public long ASMAY_Id { get; set; }
        public string msg { get; set; }
    }
}
