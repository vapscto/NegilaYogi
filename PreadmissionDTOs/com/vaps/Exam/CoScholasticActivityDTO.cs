using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class CoScholasticActivityDTO
    {

        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public bool already_cnt { get; set; }
        public int ECACT_Id { get; set; }
        public long MI_Id { get; set; }
        public string ECACT_SkillName { get; set; }
        public string ECACT_SkillCode { get; set; }
        public string EMGR_GradeName { get; set; }
        public bool ECACT_ActiveFlag { get; set; }
        public Array editlist { get; set; }
        public Array gridlist { get; set; }

        //--------------EXM_CCE_Activities_AREA
        public int ECACTA_Id { get; set; }
        public string ECACTA_SkillArea { get; set; }      
        public int ECACTA_SkillOrder { get; set; }
        public bool ECACTA_ActiveFlag { get; set; }
        public Array editlist1 { get; set; }
        public Array gridlist1 { get; set; }
        public Array getactiviteslist { get; set; }
        public Array getactivitesarealist { get; set; }
        public Array getactivitesareamappinglist { get; set; }
        public Array fillMastergrade { get; set; }
        public Temp_activiteSkillArea[] temp_activiteSkillArea { get; set; }
        public string message { get; set; }
        //---- Activites area Mapping ---//
        public long ECACTAM_Id { get; set; }
        public int EMGR_Id { get; set; }
        public bool ECACTAM_ActiveFlag { get; set; }
        public string ECACTAM_IndicatorDescription { get; set; }
        public Array getyear { get; set; }
        public string ASMAY_Year { get; set; }
        public int ASMAY_Order { get; set; }
        public long ASMAY_Id { get; set; }
    }
    public class Temp_activiteSkillArea
    {
        public long ECACTA_Id { get; set; }
        public int ECACTA_SkillOrder { get; set; }
    }
}
