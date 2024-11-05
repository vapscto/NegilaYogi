using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.TT
{
    public class TTMasterCategoryDTO
    {
        public string returnduplicatestatus;

        public long TTMC_Id { get; set; }
        public long MI_Id { get; set; }
        public string TTMC_CategoryName { get; set; }
        public long ASMAY_Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool returnval { set; get; }
        public bool TTMC_ActiveFlag { get; set; }
        public Array Categorylist { set; get; }
        public Array academiclist { set; get; }
        public Array Categorylistedit { set; get; }
        public string ASMAYYear { get; set; }          
        

    }
}
