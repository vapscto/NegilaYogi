using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeYearlyClassCategoryDTO
    {
        public long FYCC_Id { get; set; }
        public long MI_Id { get; set; }
        public long FMCC_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_ID { get; set; }
        public bool FYCC_ActiveFlag { get; set; }

        public TempClsDTO[] TempararyArrayList { get; set; }

        public bool returnval { get; set; }
        public string returnduplicatestatus { get; set; }
        public Array clsYearData { get; set; }


        public Array ClaSSCategoryArray { get; set; }
        public Array academicdrp { get; set; }
        public Array classcategorydrp { get; set; }
        public Array admclas { get; set; }

        public bool retflag { get; set; }

        public long user_id { get; set; }
    }
}
