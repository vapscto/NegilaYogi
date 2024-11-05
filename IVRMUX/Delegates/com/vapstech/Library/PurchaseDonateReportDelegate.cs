using CommonLibrary;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Library
{
  
    public class PurchaseDonateReportDelegate
    {
        CommonDelegate<CirculationParameterDTO, CirculationParameterDTO> _commbranch = new CommonDelegate<CirculationParameterDTO, CirculationParameterDTO>();
     
        public CirculationParameterDTO getdata(CirculationParameterDTO data)
        {
            return _commbranch.PostLibrary(data, "PurchaseDonateReportFacade/getdata/");
        }
    }
}
