using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class Exm_Master_Grade_DetailsDTO : CommonParamDTO
    {

        public int EMGD_Id { get; set; }
      
        public int EMGR_Id { get; set; }
        public string EMGD_Name { get; set; }
        public decimal EMGD_From { get; set; }
        public decimal EMGD_To { get; set; }
        public string EMGD_Remarks { get; set; }
        public decimal EMGD_GradePoints { get; set; }
        public bool EMGD_ActiveFlag { get; set; }
        public int count { get; set; }

    }
}
