using CommonLibrary;
using PreadmissionDTOs.com.vaps.AssetTracking;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.AssetTracking
{
    public class DisposeReportDelegate
    {
        CommonDelegate<DisposeAssetsDTO, DisposeAssetsDTO> COMAT = new CommonDelegate<DisposeAssetsDTO, DisposeAssetsDTO>();
        public DisposeAssetsDTO getloaddata(DisposeAssetsDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "DisposeReportFacade/getloaddata/");
        }
      
        public DisposeAssetsDTO getreport(DisposeAssetsDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "DisposeReportFacade/getreport/");
        }
       
        
    }
}
