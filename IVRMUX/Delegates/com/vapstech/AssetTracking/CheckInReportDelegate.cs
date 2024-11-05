using CommonLibrary;
using PreadmissionDTOs.com.vaps.AssetTracking;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.AssetTracking
{
    public class CheckInReportDelegate
    {
        CommonDelegate<CheckInAssetsDTO, CheckInAssetsDTO> COMAT = new CommonDelegate<CheckInAssetsDTO, CheckInAssetsDTO>();
        public CheckInAssetsDTO getloaddata(CheckInAssetsDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "CheckInReportFacade/getloaddata/");
        }
      
        public CheckInAssetsDTO getreport(CheckInAssetsDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "CheckInReportFacade/getreport/");
        }
       
        
    }
}
