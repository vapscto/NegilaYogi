using CommonLibrary;
using PreadmissionDTOs.com.vaps.AssetTracking.AssetTag;

namespace IVRMUX.Delegates.com.vapstech.AssetTracking.AssetTag
{
    public class AssetTagDispose_ReportDelegate
    {
        CommonDelegate<AssetTagDisposeDTO, AssetTagDisposeDTO> COMAT = new CommonDelegate<AssetTagDisposeDTO, AssetTagDisposeDTO>();
        public AssetTagDisposeDTO getloaddata(AssetTagDisposeDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AssetTagDispose_ReportFacade/getloaddata/");
        }
        public AssetTagDisposeDTO onreport(AssetTagDisposeDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AssetTagDispose_ReportFacade/onreport/");
        }

        
    }
}
