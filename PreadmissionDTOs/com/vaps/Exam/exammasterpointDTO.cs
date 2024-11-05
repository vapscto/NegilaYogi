using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class exammasterpointDTO
    {
        public long Point_Id { get; set; }
        public long MI_Id { get; set; }
        public string Point_Name { get; set; }
        public int Point_Order { get; set; }
        public bool Active_flag { get; set; }

        public Array exammasterpointname { get; set; }

        public Array exammpointname { get; set; }

        public string returnduplicatestatus { get; set; }

        public bool returnval { get; set; }

        public string retrunMsg { get; set; }

        public bool already_cnt { get; set; }

        public exammasterpointDTO[] exampointDTO { get; set; }
    }
}
