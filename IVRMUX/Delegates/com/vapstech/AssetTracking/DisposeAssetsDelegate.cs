using CommonLibrary;
using PreadmissionDTOs.com.vaps.AssetTracking;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.AssetTracking
{
    public class DisposeAssetsDelegate
    {
        CommonDelegate<DisposeAssetsDTO, DisposeAssetsDTO> COMAT = new CommonDelegate<DisposeAssetsDTO, DisposeAssetsDTO>();
        public DisposeAssetsDTO getloaddata(DisposeAssetsDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "DisposeAssetsFacade/getloaddata/");
        }
        public DisposeAssetsDTO getlocations(DisposeAssetsDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "DisposeAssetsFacade/getlocations/");
        }
        public DisposeAssetsDTO getitems(DisposeAssetsDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "DisposeAssetsFacade/getitems/");
        }
        public DisposeAssetsDTO getdetails(DisposeAssetsDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "DisposeAssetsFacade/getdetails/");
        }
        
        public DisposeAssetsDTO savedetails(DisposeAssetsDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "DisposeAssetsFacade/savedetails/");
        }
        public DisposeAssetsDTO deactive(DisposeAssetsDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "DisposeAssetsFacade/deactive/");
        }         
        
    }
}
