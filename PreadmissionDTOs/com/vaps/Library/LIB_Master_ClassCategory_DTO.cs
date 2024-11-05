using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Library
{
   public class LIB_Master_ClassCategory_DTO:CommonParamDTO
    {

        public long LMCC_Id { get; set; }
        public long MI_Id { get; set; }
        public string LMCC_CategoryName { get; set; }
        public bool LMCC_ActiveFlag { get; set; }


        public long LUCC_Id { get; set; }
        public long IVRMUL_Id { get; set; }
        public bool LUCC_ActiveFlg { get; set; }
        public long Login_Id { get; set; }
        public long HRME_Id { get; set; }


        public string HRME_EmployeeFirstName { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public Array clsscatglist { get; set; }
        public Array alldata { get; set; }
        public string message { get; set; }
        public Array stafflist { get; set; }


    }
}
