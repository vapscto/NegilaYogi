using CommonLibrary;
using PreadmissionDTOs.com.vaps.AssetTracking.AssetTag;

namespace IVRMUX.Delegates.com.vapstech.AssetTracking.AssetTag
{
    public class AssetTagCheckout_ReportDelegate
    {
        CommonDelegate<AssetTagCheckOutDTO, AssetTagCheckOutDTO> COMAT = new CommonDelegate<AssetTagCheckOutDTO, AssetTagCheckOutDTO>();
        public AssetTagCheckOutDTO getloaddata(AssetTagCheckOutDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AssetTagCheckout_ReportFacade/getloaddata/");
        }
        public AssetTagCheckOutDTO onreport(AssetTagCheckOutDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AssetTagCheckout_ReportFacade/onreport/");
        }

        
    }
}
