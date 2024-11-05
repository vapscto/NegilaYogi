using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VidyaBharathi
{
     public class VBSC_Master_UOMDTO
    {
    
        

        public long MI_Id { get; set; }
        
        public bool VBSCMUOM_ActiveFlag { get; set; }
        public Array get_uom { get; set; }
        public bool returnval { get; set; }
        public String VBCCMUOM_UOMName { get; set; }
        public long VBCCMUOM_Id { get; set; }
        public long User_Id { get; set; }

        //competition level
        public string returnduplicatestatus { get; set; }
        public string message { get; set; } 

        public long MO_Id { get; set; }
        public long VBSCMCL_Id { get; set; }
        public String VBSCMCL_CompetitionLevel { get; set; }
        public String VBSCMCL_CLDesc { get; set; }
        public bool? VBSCMCL_ActiveFlag { get; set; }
        public Array get_levl { get; set; }
        public Array get_org { get; set; }
        public string VBSCMCL_LevelFlg { get; set; }
        public long VBSCMCL_LevelOrder { get; set; }
        public string VBSCMCL_SportsCCFlg { get; set; }
        public Array Master_trust { get; set; }
        public String MO_Name { get; set;  }
        public long MT_Id { get; set; }


        
    }
}
