using PreadmissionDTOs.com.vaps.AssetTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetTrackingServiceHub.com.vaps.Interface
{
  public interface SiteLocationsReportInterface
    {
        AT_MasterSiteDTO getloaddata(AT_MasterSiteDTO data);    
        AT_MasterSiteDTO getreport(AT_MasterSiteDTO data);
        AT_MasterSiteDTO get_all_data_LCR(AT_MasterSiteDTO data);    
        AT_MasterSiteDTO getreport_LCR(AT_MasterSiteDTO data);
     
    }
}
