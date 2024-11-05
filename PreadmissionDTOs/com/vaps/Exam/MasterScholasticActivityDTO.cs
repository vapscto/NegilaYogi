using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class MasterScholasticActivityDTO
    {
       
        public long CCE_M_CoA_Id { get; set; }
        public long MI_Id { get; set; }
        public string CCE_M_CoA_Name { get; set; }
        public string CCE_M_CoA_Code { get; set; }
        public int CCE_M_CoA_Order { get; set; }
        public bool Active_flag { get; set; }
        public Array filldata { get; set; }
        public string retrunMsg { get; set; }
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public Array editlist { get; set; }
       
        public bool already_cnt { get; set; }
        public MasterScholasticActivityDTO[] subexamDTO { get; set; }
        // public MasterLifeSkillDTO[] examDTO { get; set; }

    }
    
}


