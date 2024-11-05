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
    public class AssetTagTransferDelegate
    {
        CommonDelegate<AssetTagTransferDTO, AssetTagTransferDTO> COMAT = new CommonDelegate<AssetTagTransferDTO, AssetTagTransferDTO>();
        public AssetTagTransferDTO getloaddata(AssetTagTransferDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AssetTagTransferFacade/getloaddata/");
        }

        public AssetTagTransferDTO getitems(AssetTagTransferDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AssetTagTransferFacade/getitems/");
        }
        public AssetTagTransferDTO gettolocation(AssetTagTransferDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AssetTagTransferFacade/gettolocation/");
        }
        public AssetTagTransferDTO getitemtagdata(AssetTagTransferDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AssetTagTransferFacade/getitemtagdata/");
        }
        public AssetTagTransferDTO savedata(AssetTagTransferDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AssetTagTransferFacade/savedata/");
        }
        public AssetTagTransferDTO deactive(AssetTagTransferDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AssetTagTransferFacade/deactive/");
        }


    }
}
