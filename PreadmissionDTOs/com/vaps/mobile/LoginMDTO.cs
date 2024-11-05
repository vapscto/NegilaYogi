using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.mobile
{
    public class LoginMDTO
    {
      
        public long MI_Id { get; set; }
        
        public long AMST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string ASMAY_Year { get; set; }
        public int userId { get; set; }
        public string MI_Name { get; set; }
        public string message { get; set; }
        public string roleforlogin { get; set; }
        public long empcode { get; set; }
    }
}
