using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Sports
{
    public class SPCC_Master_House_Designation_DTO:CommonParamDTO
    {
        public long SPCCMHD_Id { get; set; }
        public long MI_Id { get; set; }
        public string SPCCMHD_DesignationName {get;set;}
        public string SPCCMHD_DesignationDescription { get; set; }
       public bool SPCCMHD_ActiveFlag { get; set; }


        public bool duplicate_caste_name_bool { get; set; }
        public string msg { get; set; }
        public bool returnVal_update { get; set; }
        public Array masterhousename { get; set; }
        public Array GridviewDetails { get; set; }
        public int count { get; set; }
        public bool returnVal { get; set; }


    }
}
