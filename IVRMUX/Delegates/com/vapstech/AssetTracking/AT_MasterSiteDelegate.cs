using CommonLibrary;
using PreadmissionDTOs.com.vaps.AssetTracking;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.AssetTracking
{
    public class AT_MasterSiteDelegate
    {
        CommonDelegate<AT_MasterSiteDTO, AT_MasterSiteDTO> COMAT = new CommonDelegate<AT_MasterSiteDTO, AT_MasterSiteDTO>();
        public AT_MasterSiteDTO getloaddata(AT_MasterSiteDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AT_MasterSiteFacade/getloaddata/");
        }
        public AT_MasterSiteDTO savedetails(AT_MasterSiteDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AT_MasterSiteFacade/savedetails/");
        }
        public AT_MasterSiteDTO deactive(AT_MasterSiteDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AT_MasterSiteFacade/deactive/");
        }

    }
}
