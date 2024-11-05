using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class Student_Update_RollNumber 
    {
        public string StudentName { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public string AMST_AdmNo { get; set; }
        public long? pdays { get; set; }
        public long? AMST_Id { get; set; }
        public long? MI_Id { get; set; }
        public bool returnal { get; set; }
        public long? ASMCL_Id { get; set; }
        public long? ASMS_Id { get; set; }
        public long? ASYST_Id { get; set; }     
        public long? ASMAY_Id { get; set; }  
        public long? AMAY_RollNo { get; set; }
        public long? UserId { get; set; }

        public Student_Update_RollNumber[] studentlisturn1 { get; set; }
        public Student_Update_RollNumber[] studentlisturn { get; set; }
 
    }
}
