﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class StudentActivityDTO :CommonParamDTO
    {
        public long AMSA_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public long AMA_Id { get; set; }

    }
}