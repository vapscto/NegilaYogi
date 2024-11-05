using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Exam
{
   public class ClgMarksCalculationsDTO
    {
        public int EME_Id { get; set; }
        public long MI_Id { get; set; }
        public string EME_ExamName { get; set; }
        public Array courseslist { get; set; }
        public Array branchlist { get; set; }
        public Array examlist { get; set; }
        public Array sectionlist { get; set; }
        public Array yearlist { get; set; }
        public Array semesters { get; set; }
        public bool returnval { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long ACMS_Id { get; set; }
        public long AMSE_Id { get; set; }
    }
}
