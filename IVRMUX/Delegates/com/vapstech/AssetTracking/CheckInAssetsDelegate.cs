using CommonLibrary;
using PreadmissionDTOs.com.vaps.AssetTracking;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.AssetTracking
{
    public class CheckInAssetsDelegate
    {
        CommonDelegate<CheckInAssetsDTO, CheckInAssetsDTO> COMAT = new CommonDelegate<CheckInAssetsDTO, CheckInAssetsDTO>();
        public CheckInAssetsDTO getloaddata(CheckInAssetsDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "CheckInAssetsFacade/getloaddata/");
        }
        public CheckInAssetsDTO getStore(CheckInAssetsDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "CheckInAssetsFacade/getStore/");
        }
        public CheckInAssetsDTO getitems(CheckInAssetsDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "CheckInAssetsFacade/getitems/");
        }
        public CheckInAssetsDTO getdetails(CheckInAssetsDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "CheckInAssetsFacade/getdetails/");
        }
        public CheckInAssetsDTO savedetails(CheckInAssetsDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "CheckInAssetsFacade/savedetails/");
        }
        public CheckInAssetsDTO deactive(CheckInAssetsDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "CheckInAssetsFacade/deactive/");
        }
      

        

    }
}
