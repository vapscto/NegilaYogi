using CommonLibrary;
using PreadmissionDTOs.com.vaps.AssetTracking;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.AssetTracking
{
    public class CheckOutAssetsDelegate
    {
        CommonDelegate<CheckOutAssetsDTO, CheckOutAssetsDTO> COMAT = new CommonDelegate<CheckOutAssetsDTO, CheckOutAssetsDTO>();
        public CheckOutAssetsDTO getloaddata(CheckOutAssetsDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "CheckOutAssetsFacade/getloaddata/");
        }
        public CheckOutAssetsDTO getitems(CheckOutAssetsDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "CheckOutAssetsFacade/getitems/");
        }
        
        public CheckOutAssetsDTO savedetails(CheckOutAssetsDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "CheckOutAssetsFacade/savedetails/");
        }
        public CheckOutAssetsDTO deactive(CheckOutAssetsDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "CheckOutAssetsFacade/deactive/");
        }
        public CheckOutAssetsDTO getcontactperson(CheckOutAssetsDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "CheckOutAssetsFacade/getcontactperson/");
        }
        public CheckOutAssetsDTO get_avaiablestock(CheckOutAssetsDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "CheckOutAssetsFacade/get_avaiablestock/");
        }
        
    }
}
