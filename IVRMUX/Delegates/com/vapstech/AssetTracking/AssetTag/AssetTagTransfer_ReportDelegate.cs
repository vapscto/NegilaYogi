using CommonLibrary;
using PreadmissionDTOs.com.vaps.AssetTracking.AssetTag;

namespace IVRMUX.Delegates.com.vapstech.AssetTracking.AssetTag
{
    public class AssetTagTransfer_ReportDelegate
    {
        CommonDelegate<AssetTagTransferDTO, AssetTagTransferDTO> COMAT = new CommonDelegate<AssetTagTransferDTO, AssetTagTransferDTO>();
        public AssetTagTransferDTO getloaddata(AssetTagTransferDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AssetTagTransfer_ReportFacade/getloaddata/");
        }
        public AssetTagTransferDTO onreport(AssetTagTransferDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AssetTagTransfer_ReportFacade/onreport/");
        }

        
    }
}
