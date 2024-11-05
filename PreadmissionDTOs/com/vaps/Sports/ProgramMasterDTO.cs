using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Sports
{
    public class ProgramMasterDTO : CommonParamDTO
    {
        public long SPCCPM_Id { get; set; }
        public long MI_Id { get; set; }
        public string SPCCPM_Name { get; set; }
        public string SPCCPM_Description { get; set; }
        public bool SPCCPM_ActiveFlag { get; set; }
        public Array programList { get; set; }
        public string returnVal { get; set; }
        public Array editDetails { get; set; }

    }
}
