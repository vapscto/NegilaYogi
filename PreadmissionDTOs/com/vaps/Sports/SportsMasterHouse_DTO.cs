using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Sports
{
   public class SportsMasterHouse_DTO:CommonParamDTO
    {
        public long SPCCMH_Id { get; set; }
        public long MI_Id { get; set; }
        public string SPCCMH_HouseName { get; set; }
        public string SPCCMH_HouseDescription { get; set; }
        public bool SPCCMH_ActiveFlag { get; set; }
        public string msg { get; set; }
        public bool returnVal_update { get; set; }
        public bool duplicate_caste_name_bool { get; set; }
        public bool returnVal { get; set; }
        public Array gridviewDetails { get; set; }
        public Array editdetails { get; set; }
        public string SPCCMH_Flag { get; set; }


    }
}
