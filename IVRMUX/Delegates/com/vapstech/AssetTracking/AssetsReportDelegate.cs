using CommonLibrary;
using PreadmissionDTOs.com.vaps.AssetTracking;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.AssetTracking
{
    public class AssetsReportDelegate
    {
        CommonDelegate<CheckOutAssetsDTO, CheckOutAssetsDTO> COMAT = new CommonDelegate<CheckOutAssetsDTO, CheckOutAssetsDTO>();
        public CheckOutAssetsDTO getloaddata(CheckOutAssetsDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AssetsReportFacade/getloaddata/");
        }
      
        public CheckOutAssetsDTO getreport(CheckOutAssetsDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AssetsReportFacade/getreport/");
        }
       
        
    }
}
