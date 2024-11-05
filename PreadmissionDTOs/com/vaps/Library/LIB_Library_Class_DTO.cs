using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Library
{
   public class LIB_Library_Class_DTO:CommonParamDTO
    {
        public long LLC_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public bool LLC_ActiveFlg { get; set; }
        public long IVRMUL_Id { get; set; }

        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public long HRME_Id { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string HRME_EmployeeMiddleName { get; set; }
        public string HRME_EmployeeLastName { get; set; }

        public Array stafflist { get; set; }
        public Array classlist { get; set; }


    }
}
