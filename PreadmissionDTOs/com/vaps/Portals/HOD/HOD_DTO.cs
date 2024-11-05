using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Portals.HOD
{ 
    public class HOD_DTO
    {
        public long IHOD_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public bool IHOD_ActiveFlag { get; set; }
        public long ASMCL_Id { get; set; }
        public Array query01 { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public Array classlist { get; set; }
        public string classname { get; set; }
        public HOD_DTO[] employee { get; set; }
        public bool returnval { get; set; }
        public string returnsavestatus { get; set; }
        public Array hodlist { get; set; }
        public HOD_DTO[] class_lst { get; set; }
        public string hrme_employeeCode { get; set; }
        public Array user { get; set; }
        public HOD_DTO[]  hodclass { get; set; }
        public HOD_DTO[] hodstaff { get; set; }
        public bool IHODC_ActiveFlag { get; set; }
        public bool IHODS_ActiveFlag { get; set; }

        public Array saved_hods { get; set; }
        public Array saved_hods_cls { get; set; }
        public Array saved_hods_stf { get; set; }
        public long user_id { get; set; }
        public string IHOD_Flg { get; set; }

    }
}
