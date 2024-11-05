using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.mobile
{
    public class ExamDTO
    {
        public class input
        {
            public long AMST_Id { get; set; }
            public long MI_Id { get; set; }
            public long ASMAY_Id { get; set; }
            public long EME_Id { get; set; }
        }
        public class temp1
        {
            public int EMCA_Id { get; set; }
        }
        public class temp2
        {
            public int EME_Id { get; set; }
            public string EME_ExamName { get; set; }
        }
        public class temp3
        {
            public string grade { get; set; }
        }
        public class examid
        {          
            public Array examlist { get; set; }
        }     
        public class ExamMarks
        {
            public string SubjectName { get; set; }
            public decimal? TotalMarks { get; set; }
            public decimal? MinMarks { get; set; }
            public decimal? obtainmarks { get; set; }
        }
        
        public string subMorGFlag { get; set; }
        public Array gradname { get; set; }
        public Array Marklist { get; set; } 
    }
}
