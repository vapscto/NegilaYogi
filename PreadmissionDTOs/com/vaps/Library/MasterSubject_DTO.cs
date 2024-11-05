using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;

namespace PreadmissionDTOs.com.vaps.Library
{
    public class MasterSubject_DTO:CommonParamDTO
    {
        public long LMS_Id { get; set; }
        public long MI_Id { get; set; }
        public string LMS_SubjectName { get; set; }
        public string LMS_SubjectNo { get; set; }
        public long LMS_ParentId { get; set; }
        public long? LMS_Level { get; set; }
        public bool LMS_ActiveFlg { get; set; }
        public long ISMS_Id { get; set; }
        public string ISMS_SubjectName { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public Array parentsublist { get; set; }
        public Array alldata { get; set; }
        public string LMS_ClassNo { get; set; }

    }
}
