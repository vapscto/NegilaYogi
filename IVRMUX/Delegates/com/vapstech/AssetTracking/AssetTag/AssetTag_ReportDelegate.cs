using CommonLibrary;
using PreadmissionDTOs.com.vaps.AssetTracking.AssetTag;

namespace IVRMUX.Delegates.com.vapstech.AssetTracking.AssetTag
{
    public class AssetTag_ReportDelegate
    {
        CommonDelegate<AssetTagDTO, AssetTagDTO> COMAT = new CommonDelegate<AssetTagDTO, AssetTagDTO>();
        public AssetTagDTO getloaddata(AssetTagDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AssetTag_ReportFacade/getloaddata/");
        }
        public AssetTagDTO onreport(AssetTagDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AssetTag_ReportFacade/onreport/");
        }

        
    }
}
