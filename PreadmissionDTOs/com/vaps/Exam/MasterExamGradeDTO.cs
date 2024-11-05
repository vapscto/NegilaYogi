using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class MasterExamGradeDTO:CommonParamDTO
    {

        public bool already_cnt { get; set; }
        //master grade
        public int EMGR_Id { get; set; }
        public long MI_Id { get; set; }
        public string EMGR_GradeName { get; set; }
        public string EMGR_MarksPerFlag { get; set; }
        public bool EMGR_ActiveFlag { get; set; }
        //master grade  details
        public int EMGD_Id { get; set; }
        public string EMGD_Name { get; set; }
        public decimal EMGD_From { get; set; }
        public decimal EMGD_To { get; set; }
        public string EMGD_Remarks { get; set; }
        public decimal EMGD_GradePoints { get; set; }
        public bool EMGD_ActiveFlag { get; set; }
     
        public Exm_Master_Grade_DetailsDTO[] sub_list { get; set; }
        public Array Grade_list { get; set; }
        public Array Grade_Details_list { get; set; }
        public Array edit_m_grade { get; set; }
        public Array edit_m_grade_details { get; set; }
        public Array view_grade_details { get; set; }
        
        public string returnMsg { get; set; }
     
        public bool returnval { get; set; }

        public string returnduplicatestatus { get; set; }

   

    }
}
