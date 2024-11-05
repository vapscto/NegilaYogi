using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class ClassChangeDTO
    {
        public long ASSCOC_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id_Old { get; set; }
        public long ASMCL_Id_New { get; set; }
        public string ASSCOC_Remarks { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long CreatedBy { get; set; }
        public long UpdatedBy { get; set; }

        public long MI_Id { get; set; }
        public long user_id { get; set; }

        public Array dataclass { get; set; }

        public Array academicList { get; set; }
    }
}
