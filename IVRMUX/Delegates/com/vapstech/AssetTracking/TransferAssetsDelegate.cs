using CommonLibrary;
using PreadmissionDTOs.com.vaps.AssetTracking;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.AssetTracking
{
    public class TransferAssetsDelegate
    {
        CommonDelegate<TransferAssetsDTO, TransferAssetsDTO> COMAT = new CommonDelegate<TransferAssetsDTO, TransferAssetsDTO>();
        public TransferAssetsDTO getloaddata(TransferAssetsDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "TransferAssetsFacade/getloaddata/");
        }
        public TransferAssetsDTO gettolocations(TransferAssetsDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "TransferAssetsFacade/gettolocations/");
        }
        public TransferAssetsDTO getitemdetails(TransferAssetsDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "TransferAssetsFacade/getitemdetails/");
        }     
        public TransferAssetsDTO savedetails(TransferAssetsDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "TransferAssetsFacade/savedetails/");
        }
        public TransferAssetsDTO deactive(TransferAssetsDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "TransferAssetsFacade/deactive/");
        }         
        
    }
}
