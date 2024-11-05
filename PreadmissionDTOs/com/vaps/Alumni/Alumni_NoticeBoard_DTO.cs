using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Alumni
{
    public class Alumni_NoticeBoard_DTO
    {
        public long ALNTB_Id { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long ASMAY_Id { get; set; }
        public long IVRMRT_Id { get; set; }
        public string Role_flag { get; set; }
        public string message { get; set; }
      
        public string ALNTB_Title { get; set; }
        public string ALNTB_Description { get; set; }
        public string ALNTB_FilePath { get; set; }
        public string ALNTB_Attachment { get; set; }
        public DateTime? ALNTB_DisplayDate { get; set; }
        public DateTime? ALNTB_StartDate { get; set; }
        public DateTime? ALNTB_EndDate { get; set; }
        public string ALNTB_TTSylabusFlg { get; set; }
        public bool ALNTB_ActiveFlag { get; set; }
        public DateTime? ALNTB_CreatedDate { get; set; }
        public DateTime? ALNTB_UpdatedDate { get; set; }
        public Array alumninoticeboardlist { get; set; }
        public Array attachementlist { get; set; }
        public Array editdetailsfiles { get; set; }
        public Array editdetails { get; set; }
        public Attachment_Array1[] Attachment_Array { get; set; }
        public class Attachment_Array1
        {
            public string ALNTBFL_FileName { get; set; }
            public string ALNTBFL_FilePath { get; set; }
        }
    }
}
