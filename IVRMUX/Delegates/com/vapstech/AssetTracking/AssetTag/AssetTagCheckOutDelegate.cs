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
    public class AssetTagCheckOutDelegate
    {
        CommonDelegate<AssetTagCheckOutDTO, AssetTagCheckOutDTO> COMAT = new CommonDelegate<AssetTagCheckOutDTO, AssetTagCheckOutDTO>();
        public AssetTagCheckOutDTO getloaddata(AssetTagCheckOutDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AssetTagCheckOutFacade/getloaddata/");
        }
        
            public AssetTagCheckOutDTO getitems(AssetTagCheckOutDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AssetTagCheckOutFacade/getitems/");
        }
        public AssetTagCheckOutDTO getitemtagdata(AssetTagCheckOutDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AssetTagCheckOutFacade/getitemtagdata/");
        }
        public AssetTagCheckOutDTO savedata(AssetTagCheckOutDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AssetTagCheckOutFacade/savedata/");
        }
        public AssetTagCheckOutDTO deactive(AssetTagCheckOutDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AssetTagCheckOutFacade/deactive/");
        }
        

    }
}
