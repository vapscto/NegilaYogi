using CommonLibrary;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Library.Reports
{
    public class OpacSearchDelegate
    {
        CommonDelegate<OpacSearchDTO, OpacSearchDTO> comm = new CommonDelegate<OpacSearchDTO, OpacSearchDTO>();
        public OpacSearchDTO getalldetails(OpacSearchDTO data)
        {
            return comm.PostLibrary(data, "OpacSearchFacade/getalldetails/");
        }
        public OpacSearchDTO report(OpacSearchDTO data)
        {
            return comm.PostLibrary(data, "OpacSearchFacade/report");
        }
    }
}
