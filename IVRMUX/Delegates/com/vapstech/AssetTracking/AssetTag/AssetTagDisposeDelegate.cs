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
    public class AssetTagDisposeDelegate
    {
        CommonDelegate<AssetTagDisposeDTO, AssetTagDisposeDTO> COMAT = new CommonDelegate<AssetTagDisposeDTO, AssetTagDisposeDTO>();
        public AssetTagDisposeDTO getloaddata(AssetTagDisposeDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AssetTagDisposeFacade/getloaddata/");
        }

        public AssetTagDisposeDTO getlocation(AssetTagDisposeDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AssetTagDisposeFacade/getlocation/");
        }
        public AssetTagDisposeDTO getitems(AssetTagDisposeDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AssetTagDisposeFacade/getitems/");
        }
        public AssetTagDisposeDTO getitemtagdata(AssetTagDisposeDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AssetTagDisposeFacade/getitemtagdata/");
        }
        public AssetTagDisposeDTO savedata(AssetTagDisposeDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AssetTagDisposeFacade/savedata/");
        }
        public AssetTagDisposeDTO deactive(AssetTagDisposeDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AssetTagDisposeFacade/deactive/");
        }


    }
}
