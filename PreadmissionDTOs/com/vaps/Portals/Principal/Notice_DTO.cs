using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Portals.Principal
{
    public class Notice_DTO:CommonParamDTO
    {
        public long INTB_Id { get; set; }
        public long MI_Id { get; set; }
        public string INTB_Title { get; set; }
        public string INTB_Description { get; set; }
        public string INTB_Attachment { get; set; }
        public DateTime? INTB_DisplayDate { get; set; }
        public bool INTB_ActiveFlag { get; set; }
        public DateTime? INTB_StartDate { get; set; }
        public DateTime? INTB_EndDate { get; set; }
        public string returnsavestatus { get; set; }
        public bool returnval { get; set; }
        public string imagePath { get; set; }
        public Array notice_data { get; set; }
        public Notice_DTO[] images_list { get; set; }

    }
}
