using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class tempDTO
    {
       public long amsT_Id { get; set; }
        public string amsT_FirstName { get; set; }
        public tempDTO1[] sub_list { get; set; }
    }
    public class tempDTO1
    {
        public long id { get; set; }
        public string name { get; set; }
    }
}
