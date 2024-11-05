using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Sports
{
    public class SportMasterCompitionLevelDTO
    {
        public long SPCCMCL_Id { get; set; }
        public long MI_Id { get; set; }
        public string SPCCMCL_CompitionLevel { get; set; }
        public string SPCCMCL_CLDesc { get; set; }
        public bool SPCCMCL_ActiveFlag { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Array mastercompitionname { get; set; }
        public Array GridviewDetails { get; set; }
        public int count { get; set; }
        public string msg { get; set; }
        public bool returnVal { get; set; }
        public bool returnVal_update { get; set; }
        public bool duplicate_compition_name_bool { get; set; }
    }
}
