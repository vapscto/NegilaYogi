using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VidyaBharathi
{
   public class VBSC_Events_Category_StudentsDTO
    {
        public long MI_Id { get; set; }
        public long MT_Id { get; set; }
        public long User_Id { get; set; }

        public long VBSCECTSTU_Id { get; set; }
        public long VBSCECT_Id { get; set; }
        public long AMST_ID { get; set; }
        public string AMST_Name { get; set; }
        public long ASMCL_ID { get; set; }
        public long ASMS_Id { get; set; }
        public long VBSCECTSTU_Rank { get; set; }
        public decimal VBSCECTSTU_Points { get; set; }
        public bool VBSCECTSTU_RecordBrokenFlag { get; set; }
        public string VBSCECTSTU_Remarks { get; set; }
        public bool VBSCECTSTU_ActiveFlag { get; set; }
        public DateTime? VBSCECTSTU_CreatedDate { get; set; }
        public DateTime? VBSCECTSTU_UpdatedDate { get; set; }
        public long VBSCECTSTU_CreatedBy { get; set; }
        public long VBSCECTSTU_UpdatedBy { get; set; }


        public long VBSCME_Id { get; set; }
        public string VBSCME_EventName { get; set; }
        public Array geteventCategory { get; set; }


        public string returnduplicatestatus { get; set; }
        public bool already_cnt { get; set; }
        public string message { get; set; }
        public bool returnval { get; set; }





    }
}
