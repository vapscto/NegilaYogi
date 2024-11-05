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
    public class AssetTagCheckInDelegate
    {
        CommonDelegate<AssetTagCheckInDTO, AssetTagCheckInDTO> COMAT = new CommonDelegate<AssetTagCheckInDTO, AssetTagCheckInDTO>();
        public AssetTagCheckInDTO getloaddata(AssetTagCheckInDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AssetTagCheckInFacade/getloaddata/");
        }

        public AssetTagCheckInDTO getstore(AssetTagCheckInDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AssetTagCheckInFacade/getstore/");
        }
        public AssetTagCheckInDTO getitems(AssetTagCheckInDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AssetTagCheckInFacade/getitems/");
        }
        public AssetTagCheckInDTO getitemtagdata(AssetTagCheckInDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AssetTagCheckInFacade/getitemtagdata/");
        }
        public AssetTagCheckInDTO savedata(AssetTagCheckInDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AssetTagCheckInFacade/savedata/");
        }
        public AssetTagCheckInDTO deactive(AssetTagCheckInDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AssetTagCheckInFacade/deactive/");
        }


    }
}
