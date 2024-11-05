using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.College.Admission
{
    public class TotalSeatAllotmentDelegates
    {

        CommonDelegate<TotalSeatAllotmentDTO, TotalSeatAllotmentDTO> common = new CommonDelegate<TotalSeatAllotmentDTO, TotalSeatAllotmentDTO>();
      

        public TotalSeatAllotmentDTO onreport(TotalSeatAllotmentDTO id)
        {
            return common.clgadmissionbypost(id, "TotalSeatAllotmentFacade/onreport/");
        }

    }
}
