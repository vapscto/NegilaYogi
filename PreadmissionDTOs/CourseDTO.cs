using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class CourseDTO : CommonParamDTO
    {
        public int AMCO_Id { get; set; }
        public string AMCO_Name { get; set; }
        public string AMCO_Details { get; set; }
        public string AMCo_Flag { get; set; }
        public string AMCO_Code { get; set; }
        public int AMCO_Min_Atten_Per { get; set; }
        public int AMCO_No_Year { get; set; }
        public int AMCO_Fee_App_Type { get; set; }
        public int AMC_ID { get; set; }
        public string AMC_MOS { get; set; }
        public int AMCO_Order { get; set; }
    }
}
