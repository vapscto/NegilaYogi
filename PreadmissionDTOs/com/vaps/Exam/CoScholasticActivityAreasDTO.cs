using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class CoScholasticActivityAreasDTO : CommonParamDTO
    {

        //public bool already_cnt { get; set; }
        //master category
        public int CCE_M_Sch_Area_Id { get; set; }
        public long MI_Id { get; set; }
        public string CCE_M_Sch_Area_Name { get; set; }
        public int CCE_M_Sch_Area_Order { get; set; }
        public bool Active_flag { get; set; }
        public bool returnval { get; set; }
        public bool already_cnt { get; set; }
        public string retrunMsg { get; set; }
        public Array exammastername { get; set; }
        public string returnduplicatestatus { get; set; }
        public CoScholasticActivityAreasDTO[] examDTO { get; set; }
    }        
}
