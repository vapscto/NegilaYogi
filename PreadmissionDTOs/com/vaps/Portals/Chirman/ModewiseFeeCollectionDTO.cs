using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Portals.Chirman
{
    public class ModewiseFeeCollectionDTO
    {
        public int EME_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public Array Smscount { get; set; }
        public Array Emailcount { get; set; }
        public long totalsms { get; set; }
        public long totalEmail { get; set; }
        public int classid { get; set; }
        
        public long asmS_Id { get; set; }
        public long asmcL_Id { get; set; }
        public Array Fillstudentstrenth { get; set; }
        public Array selectedyear { get; set; }
        public Array yearlist { get; set; }
        public Array fillabsent { get; set; }
        public Array fillfee { get; set; }
        public Array coedata { get; set; }
        public Array newadmstd { get; set; }
        public Array fillregstd { get; set; }
        public Array fillnewadmstd { get; set; }
        public Array sectionwisestrenth { get; set; }

        public Array classarray { get; set; }
        public Array sectionarray { get; set; }
        public Array sectionwisestrenthnewadm { get; set; }
        


        public int sectionid { get; set; }
        public string sectionname { get; set; }
        public string eventName { get; set; }
        public string year { get; set; }
        public string eventDesc { get; set; }
        public DateTime? COEE_EStartDate { get; set; }
        public DateTime? COEE_EEndDate { get; set; }
       
        public string feeclass { get; set; }
        public string Class_Name { get; set; }
        public long stud_count { get; set; }
        public string EME_ExamName { get; set; }
        public string EME_ExamCode { get; set; }
        public int EME_ExamOrder { get; set; }

        public string NameOfDesig { get; set; }
        public long absentee { get; set; }
        public decimal ballance { get; set; }
        public decimal recived { get; set; }
        public decimal paid { get; set; }
        public decimal concession { get; set; }
        public long user_id { get; set; }



    }
}


