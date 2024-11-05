using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Exam
{
   public class ExamWiseTermReport_DTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public Array yearlist { get; set; }
        public Array classlist { get; set; }
        public Array sectionlist { get; set; }
        public Array examlist { get; set; }
        public Array exmstdlist { get; set; }
        public Array Staffmobileappprivileges { get; set; }
        public string Pagename { get; set; }
        public string Pageicon { get; set; }
        public string Pageurl { get; set; }
        public long? IVRMRMAP_Id { get; set; }
        public bool? IVRMMAP_AddFlg { get; set; }
        public bool? IVRMMAP_UpdateFlg { get; set; }
        public bool? IVRMMAP_DeleteFlg { get; set; }
        public string mobileprivileges { get; set; }
        public string stringmobileorportal { get; set; }
        public long Roleid { get; set; }
        public long Userid { get; set; }
        public string UserName { get; set; }
        public string flag { get; set; }

    }
}
