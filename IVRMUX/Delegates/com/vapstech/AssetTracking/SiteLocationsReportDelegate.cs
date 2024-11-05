using CommonLibrary;
using PreadmissionDTOs.com.vaps.AssetTracking;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.AssetTracking
{
    public class SiteLocationsReportDelegate
    {
        CommonDelegate<AT_MasterSiteDTO, AT_MasterSiteDTO> COMAT = new CommonDelegate<AT_MasterSiteDTO, AT_MasterSiteDTO>();
        public AT_MasterSiteDTO getloaddata(AT_MasterSiteDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "SiteLocationsReportFacade/getloaddata/");
        }
      
        public AT_MasterSiteDTO getreport(AT_MasterSiteDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "SiteLocationsReportFacade/getreport/");
        }
        public AT_MasterSiteDTO get_all_data_LCR(AT_MasterSiteDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "SiteLocationsReportFacade/get_all_data_LCR/");
        }
      
        public AT_MasterSiteDTO getreport_LCR(AT_MasterSiteDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "SiteLocationsReportFacade/getreport_LCR/");
        }
       
        
    }
}
