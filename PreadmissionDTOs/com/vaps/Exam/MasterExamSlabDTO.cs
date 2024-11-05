using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class MasterExamSlabDTO : CommonParamDTO
    {

        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public Array GetDetails { get; set; }
        public SlabList[] SlabLists { get; set; }
        public string returnval { get; set; }
    }
    public class SlabList
    {
        public decimal EMPTSL_PercentFrom { get; set; }
        public decimal EMPTSL_PercentTo { get; set; }
        public string EMPTSL_Points { get; set; }
    }
}
