using CommonLibrary;
using PreadmissionDTOs.com.vaps.AssetTracking;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.AssetTracking
{
    public class AT_MasterLocationDelegate
    {
        CommonDelegate<AT_MasterLocationDTO, AT_MasterLocationDTO> COMAT = new CommonDelegate<AT_MasterLocationDTO, AT_MasterLocationDTO>();
        public AT_MasterLocationDTO getloaddata(AT_MasterLocationDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AT_MasterLocationFacade/getloaddata/");
        }
        public AT_MasterLocationDTO savedetails(AT_MasterLocationDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AT_MasterLocationFacade/savedetails/");
        }
        public AT_MasterLocationDTO deactive(AT_MasterLocationDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AT_MasterLocationFacade/deactive/");
        }
              
    }
}
