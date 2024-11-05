using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission
{
    public class NAACAlumniMeetingDTO
    {
        public long NCAC543ALMMET_Id { get; set; }
        public long NCAC543ALMMETF_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCAC543ALMMET_MeetingYear { get; set; }
        public long NCAC543ALMMET_NoOfMeetings { get; set; }
        public DateTime NCAC543ALMMET_MeetingDate { get; set; }
        public long NCAC543ALMMET_NoOfMemAttnd { get; set; }
        public long NCAC543ALMMET_TotalAlumniCount { get; set; }
        public bool NCAC543ALMMET_ActiveFlg { get; set; }
        public long NCAC543ALMMET_CreatedBy { get; set; }
        public long NCAC543ALMMET_UpdatedBy { get; set; }
        public DateTime NCAC543ALMMET_CreatedDate { get; set; }
        public DateTime NCAC543ALMMET_UpdatedDate { get; set; }


        public long UserId { get; set; }

        public string ASMAY_Year { get; set; }
        public string NCAC543ALMMET_StatusFlg { get; set; }

        public Array institutionlist { get; set; }
        public Array allacademicyear { get; set; }
        public Array alldatalist { get; set; }
        public Array editlist { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public Array editfiles { get; set; }

        public NAACCriteriaFivefileDTO[] filelist { get; set; }
        public string Remarks { get; set; }
        public long filefkid { get; set; }
        public Array commentlist { get; set; }
    }
}
