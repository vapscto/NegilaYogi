using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class GradeSlabReportDTO
    {
        public int EMGR_Id { get; set; }
        public long MI_Id { get; set; }
        public string EMGR_GradeName { get; set; }
        public string EMGR_MarksPerFlag { get; set; }
        public bool EMGR_ActiveFlag { get; set; }
        public int EMGD_Id { get; set; }
        
        public string EMGD_Name { get; set; }
        public decimal EMGD_From { get; set; }
        public decimal EMGD_To { get; set; }
        public string EMGD_Remarks { get; set; }
        public decimal EMGD_GradePoints { get; set; }
        public bool EMGD_ActiveFlag { get; set; }

        public Array yearlist { get; set; }
        public Array grlist { get; set; }
        public Array groupname { get; set; }
        public Array categoryname { get; set; }
        public Array subjectname { get; set; }
        public Array studentAttendanceList { get; set; }
        public Array masterinstitution { get; set; }

        public string type { get; set; }


    }
}
