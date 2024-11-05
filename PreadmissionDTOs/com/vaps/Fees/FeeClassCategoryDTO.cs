using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeClassCategoryDTO
    {
        public long FMCC_Id { get; set; }
        public long MI_Id { get; set; }
        public string FMCC_ClassCategoryName { get; set; }
        public string FMCC_ClassCategoryCode { get; set; }
        public bool FMCC_ActiveFlag { get; set; }


        public bool returnval { get; set; }
        public string returnduplicatestatus { get; set; }
        public Array ClaSSCategoryArray { get; set; }
        public Array academicdrp { get; set; }
        public Array classcategorydrp { get; set; }
        public Array admclas { get; set; }
        public Array clsYearData { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMAY_Year { get; set; }
        public long FYCCC_Id { get; set; }

        public long FYCC_Id { get; set; }

        public long ASMAY_ID { get; set; }
        public long ASMCL_ID { get; set; }

        public bool retvalue { get; set; }

        //     public TempClsDTO[] TempararyArrayList { get; set; }

        public string message { get; set; }

        public long user_id { get; set; }
    }
}
