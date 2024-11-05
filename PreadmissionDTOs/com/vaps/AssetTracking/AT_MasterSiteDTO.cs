using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.AssetTracking
{
    public class AT_MasterSiteDTO
    {
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public bool already_cnt { get; set; }
        public string message { get; set; }
        public long INVMSI_Id { get; set; }
        public long MI_Id { get; set; }
        public string INVMSI_SiteBuildingName { get; set; }
        public string INVMSI_SiteRemarks { get; set; }
        public bool INVMSI_ActiveFlg { get; set; }
        public Array get_sites { get; set; }
        public Array get_sitereport { get; set; }
        public Array location_list { get; set; }
        public Array location_print_list { get; set; }
        public sitearrayDTO[] sitearray { get; set; }
        public selecteloc_list1[] selecteloc_list { get; set; }

    }

    public class sitearrayDTO
    {
        public long INVMSI_Id { get; set; }
        public string INVMSI_SiteBuildingName { get; set; }
    }
    public class selecteloc_list1
    {
        public long INVMLO_Id { get; set; }
    }
}
