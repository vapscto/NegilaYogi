using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class ECSBulkImportDTO
    {
        public long ASMAY_Id { set; get; }
        public long MI_Id { set; get; }
        public Array yearlist  { set; get; }
        public long userid { get; set; }
        public FeeECSDTO[] newlstget1 { get; set; }
        public string stuStatus { get; set; }
        public string SMS { get; set; }
        public string Email { get; set; }
         
    }
}
