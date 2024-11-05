using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class MasterLifeSkillDTO
    {

        public int ECS_Id { get; set; }
        public long MI_Id { get; set; }
        public string ECS_SkillName { get; set; }
        public string ECS_SkillCode { get; set; }
        public bool ECS_ActiveFlag { get; set; }
        public string message { get; set; }
        public bool returnval { get; set; }
        public Array editlist { get; set; }
        public Array filldata { get; set; }
      
        //MAster Life Skill Area
        public int ECSA_Id { get; set; }
        public string ECSA_SkillArea { get; set; }
        public int ECSA_SkillOrder { get; set; }
        public bool ECSA_ActiveFlag { get; set; }
        public bool already_cnt { get; set; }
        public Array getskilldata { get; set; }
        public Temp_LifeSkillArea[] subexamDTO { get; set; }

        //MAster Life Skill Area Mapping
        public Array filldatamapping { get; set; }
        public Array fillskill { get; set; }
        public Array fillskillarea { get; set; }
        public Array fillMastergrade { get; set; }
        public long ECSAM_Id { get; set; }           
        public string ECSAM_IndicatorDescription { get; set; }
        public int EMGR_Id { get; set; }
        public bool ECSAM_ActiveFlag { get; set; }
        public int EMGD_Id { get; set; }
        public string EMGR_NAME { get; set; }
        public string EMGR_Point { get; set; }
        public Array fillgradename { get; set; }
        public Array getyear { get; set; }
        public string ASMAY_Year { get; set; }
        public int ASMAY_Order { get; set; }
        public long ASMAY_Id { get; set; }
    }
    public class Temp_LifeSkillArea
    {
        public long ECSA_Id { get; set; }
        public int ECSA_SkillOrder { get; set; }
    }
   
}


