using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.AssetTracking
{
    public class AT_MasterLocationDTO
    {
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public bool already_cnt { get; set; }
        public string message { get; set; }
        public long INVMLO_Id { get; set; }
        public long MI_Id { get; set; }
        public long INVMSI_Id { get; set; }
        public string INVMLO_LocationRoomName { get; set; }
        public string INVMLO_LocationRemarks { get; set; }
        public bool INVMLO_ActiveFlg { get; set; }
        public string INVMSI_SiteBuildingName { get; set; }
        public string INVMSI_SiteRemarks { get; set; }
        public long? HRME_Id { get; set; }
        public string INVMLO_InchargeName { get; set; }
        public string employeename { get; set; }    
        public int? HRME_EmployeeOrder { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public string contactpersonflag { get; set; }
        public string contactflag { get; set; }
        public Array get_sites { get; set; }
        public Array get_locations { get; set; }
        public Array get_employee { get; set; }
        public Array get_contactperson { get; set; }
        


    }
}
