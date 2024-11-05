using CommonLibrary;
using PreadmissionDTOs.com.vaps.AssetTracking;
using PreadmissionDTOs.com.vaps.AssetTracking.AssetTag;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.AssetTracking.AssetTag
{
    public class AssetTagDelegate
    {
        CommonDelegate<AssetTagDTO, AssetTagDTO> COMAT = new CommonDelegate<AssetTagDTO, AssetTagDTO>();
        public AssetTagDTO getloaddata(AssetTagDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AssetTagFacade/getloaddata/");
        }
        public AssetTagDTO getdata(AssetTagDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AssetTagFacade/getdata/");
        }
        public AssetTagDTO savedata(AssetTagDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AssetTagFacade/savedata/");
        }
        public AssetTagDTO deactive(AssetTagDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AssetTagFacade/deactive/");
        }
        

    }
}
