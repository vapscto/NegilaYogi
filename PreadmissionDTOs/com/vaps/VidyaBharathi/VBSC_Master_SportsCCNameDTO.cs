using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VidyaBharathi
{
    public class VBSC_Master_SportsCCNameDTO
    {
        public long MI_Id { get; set; }
        public string message { get; set; }
        public long VBSCMSCC_Id { get; set; }
        public long VBSCMSCCG_Id { get; set; }
        public long MT_Id { get; set; }
        public string VBSCMSCC_SportsCCName { get; set; }
        public string VBSCMSCC_SportsCCDesc { get; set; }
        public string VBSCMSCC_SGFlag { get; set; }
        public long VBSCMSCC_NoOfMembers { get; set; }
        public string VBSCMSCC_RecHighLowFlag { get; set; }
        public string VBSCMSCC_RecInfo { get; set; }
        public string VBSCMSCC_GenderFlg { get; set; }
        public bool VBSCMSCC_ActiveFlag { get; set; }
        public Array getGroupName { get; set; }
        public Array Master_trust { get; set; }
        public Array get_customer { get; set; }
        public string MO_Name { get; set; }
        public string VBSCMSCCG_SportsCCGroupName { get; set; }
        public string VBSCMSCCG_SCCFlag { get; set; }
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public DateTime? VBSCMSCC_CreatedDate { get; set; }
        public DateTime? VBSCMSCC_UpdatedDate { get; set; }

    }
}
