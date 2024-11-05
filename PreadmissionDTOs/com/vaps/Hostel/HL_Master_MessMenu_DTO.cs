using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PreadmissionDTOs.com.vaps.Hostel
{
    public class HL_Master_MessMenu_DTO
    {
        public long HLMMN_Id { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long HLMM_Id { get; set; }
        public long HLMMC_Id { get; set; }
        public string HLMMN_MenuName { get; set; }
        public string HLMMN_MenuDesc { get; set; }
        public bool HLMMN_ActiveFlag { get; set; }
        public string HLMM_Name { get; set; }
        public string HLMMC_Name { get; set; }

        public bool duplicate { get; set; }
        public bool returnval { get; set; }

        public Array get_messCategorylist{get;set;}
        public Array get_messlist { get;set;}
        public Array edit_MessMenulist { get;set;}
        public Array griddata { get;set;}
    }
}
