using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager.PettyCash
{
    public class PC_Master_ParticularsDTO
    {
        public long MI_Id { get; set; }
        public long Userid { get; set; }
        public long roleid { get; set; }
        public Array getloaddetails { get; set; }
        public long PCMPART_Id { get; set; }
        public string PCMPART_ParticularName { get; set; }
        public string PCMPART_ParticularDesc { get; set; }
        public bool PCMPART_ActiveFlg { get; set; }
        public bool returnval { get; set; }
        public string message { get; set; }
    }
}
