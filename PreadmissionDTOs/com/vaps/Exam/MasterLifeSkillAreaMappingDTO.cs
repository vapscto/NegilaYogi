using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class MasterLifeSkillAreaMappingDTO
    {
        public long CCE_MLSAMap_id { get; set; }
        public long CCE_MLS_ID { get; set; }
        public string CCE_MLS_NAME { get; set; }
        public long CCE_MLSA_ID { get; set; }
        public string CCE_MLSA_NAME { get; set; }
        public string CCE_Indicator_Description { get; set; }
        public int EMGD_Id { get; set; }
        public bool CCE_MLSAMap_ActiveFlag { get; set; }
        public long MI_Id { get; set; }
        public string EMGR_NAME { get; set; }
        public string EMGR_Point { get; set; }

        public Array fillskill { get; set; }
        public Array fillskillarea { get; set; }
        public Array fillgradename { get; set; }
        public Array filldata { get; set; }
        public string retrunMsg { get; set; }
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public Array editlist { get; set; }
        public Array fillMastergrade { get; set; }
        public int EMGR_Id { get; set; }


        // public MasterLifeSkillDTO[] examDTO { get; set; }

    }
    
}


