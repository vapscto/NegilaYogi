using CommonLibrary;
using PreadmissionDTOs.com.vaps.AssetTracking.AssetTag;

namespace IVRMUX.Delegates.com.vapstech.AssetTracking.AssetTag
{
    public class AssetTagCheckIn_ReportDelegate
    {
        CommonDelegate<AssetTagCheckInDTO, AssetTagCheckInDTO> COMAT = new CommonDelegate<AssetTagCheckInDTO, AssetTagCheckInDTO>();
        public AssetTagCheckInDTO getloaddata(AssetTagCheckInDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AssetTagCheckIn_ReportFacade/getloaddata/");
        }
        public AssetTagCheckInDTO onreport(AssetTagCheckInDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AssetTagCheckIn_ReportFacade/onreport/");
        }

        
    }
}
