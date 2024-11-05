using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VidyaBharathi
{
   public class VBSC_Events_CategoryDTO
    {
        public long MI_Id { get; set; }
        public long User_Id { get; set; }

        public long VBSCECT_Id { get; set; }
        public long VBSCME_Id { get; set; }
        public long VBSCMCC_Id { get; set; }
        public long VBSCMSCC_Id { get; set; }
        public bool VBSCECT_GroupActivityFlg { get; set; }
        public long VBSCECT_MaxNoOfGroup { get; set; }
        public long VBSCECT_MaxNoOfStudents { get; set; }
        public String VBSCECT_Remarks { get; set; }
        public bool VBSCECT_ActiveFlag { get; set; }
        public DateTime? VBSCECT_CreatedDate { get; set; }
        public DateTime? VBSCECT_UpdatedDate { get; set; }
        public long VBSCECT_CreatedBy { get; set; }
        public long VBSCECT_UpdatedBy { get; set; }

        public Array getcompitionCategory { get; set; }
        public Array getSportsCCName { get; set; }
        public Array getMasterEvents { get; set; }
        public Array geteventcategory { get; set; }

        public string VBSCME_EventName { get; set; }
        public string VBSCMCC_CompetitionCategory { get; set; }
        public string VBSCMSCC_SportsCCName { get; set; }


        public string returnduplicatestatus { get; set; }
        public bool already_cnt { get; set; }
        public string message { get; set; }
        public bool returnval { get; set; }





    }
}
