using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class MasterLifeSkillAreaDTO
    {
       
        public long CCE_MLSA_ID { get; set; }
        public long MI_Id { get; set; }
        public int CCE_MLSA_Order { get; set; }
        public string CCE_MLSA_NAME { get; set; }
        public Array filldata { get; set; }
        public string retrunMsg { get; set; }
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public Array editlist { get; set; }
        public bool CCE_MLSA_ActiveFlag { get; set; }
        public bool already_cnt { get; set; }
        public MasterLifeSkillAreaDTO[] subexamDTO { get; set; }

    }
    
}


