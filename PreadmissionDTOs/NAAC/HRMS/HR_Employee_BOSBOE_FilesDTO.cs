﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.HRMS
{
    public class HR_Employee_BOSBOE_FilesDTO
    {
        public long NCHREBOSF_Id { get; set; }
        public string NCHREBOSF_FileName { get; set; }
        public string NCHREBOSF_Filedesc { get; set; }
        public string NCHREBOSF_FilePath { get; set; }
        public long HREBOS_Id { get; set; }
        public bool NCHREBOSF_ApprovedFlg { get; set; }
        public string NCHREBOSF_Remarks { get; set; }
        public string NCHREBOSF_StatusFlg { get; set; }
        public bool NCHREBOSF_ActiveFlag { get; set; }
    }
}
