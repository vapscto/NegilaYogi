using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class ExamMarksImportDTO
    {
        public long AMST_Id { get; set; }
        public string Student_Name { get; set; }
        public string Admission_No { get; set; }
        public string Roll_No { get; set; }
        public string Max_Marks { get; set; }
        public string Min_Marks { get; set; }
        public string Obtain_Marks { get; set; }


        //public List<ExamMarksImportDTO> newlstget { get; set; }
    }
}
