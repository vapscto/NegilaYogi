using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.admission
{
   public class Master_ExamQualified_ClassDTO
    {
        public long MI_Id { get; set; }
        public long User_id { get; set; }
        public string IMQC_ExamName { get; set; }
        public string message { get; set; }
        public long IMQC_Id { get; set; }
        public bool IMQC_ActiveFlag { get; set; }
        public string returnval { get; set; }

        public DateTime IMQC_CreatedDate { get; set; }
        public DateTime IMQC_UpdatedDate { get; set; }
       public long IMQC_CreatedBy { get; set; }
        public long IMQC_UpdatedBy  { get; set; }
        public Array ExamQualifiedClass { get; set; }
        public Array EditExamQualifiedClass { get; set; }



    }
}
