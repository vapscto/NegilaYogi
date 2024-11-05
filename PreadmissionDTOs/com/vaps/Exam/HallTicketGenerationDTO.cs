using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class HallTicketGenerationDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long EME_Id { get; set; }
        public Array Acdlist { get; set; }       
        public Array ctlist { get; set; }
        public Array seclist { get; set; }
        public Array examlist { get; set; }
        public Array alldata { get; set; }
        public string prefixstr { get; set; }
        public int startno { get; set; }
        public int increment { get; set; }
        public int leadingzeros { get; set; }
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public Array datareport { get; set; }
        public HallTicketGenerationDTO[] section_Array { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public int ASMCL_Order { get; set; }
        public int ASMC_Order { get; set; }
        public long EHT_Id { get; set; }
        public string ASMAY_Year { get; set; }
        public string EME_ExamName { get; set; }
        public int ASMAY_Order { get; set; }
        public long AMST_Id { get; set; }
        public string AMST_FirstName { get; set; }
        public string AMST_AdmNo { get; set; }
        public string EHT_HallTicketNo { get; set; }
        public string AMST_Photoname { get; set; }
        public string AMST_FatherName { get; set; }
        public string statusflag { get; set; }
        public long AMAY_Rollno { get; set; }
        public bool? EHT_PublishFlg { get; set; }
        public selectedstudents[] selectedstudents { get; set; }
    }

    public class selectedstudents
    {
        public long AMST_Id { get; set; }
        public long EHT_Id { get; set; }
    }
}
