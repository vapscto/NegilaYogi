using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Sports
{
    public class SportMasterUOMDTO
    {
        public long SPCCMUOM_Id { get; set; }
        public long MI_Id { get; set; }
        public string SPCCMUOM_UOMName { get; set; }
        public bool SPCCMUOM_ActiveFlag { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Array masterUOMname { get; set; }
        public Array GridviewDetails { get; set; }
        public int count { get; set; }
        public string msg { get; set; }
        public bool returnVal { get; set; }
        public bool returnVal_update { get; set; }
        public bool duplicate_UOM_name_bool { get; set; }
    }
}
