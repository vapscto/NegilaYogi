﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Admission
{
    public class CollegeImportStudentWrapDTO
    {
        public Int64 MI_Id { get; set; }
        public Int64 ASMAY_Id { get; set; }
        public string stuStatus { get; set; }    
        public string returnMsg { get; set; }
        public string resp { get; set; }
        public long useridapp { get; set; }
        public long useridappfat { get; set; }
        public long useridappmot { get; set; }
        public Array failedlist { get; set; }
    }
}
