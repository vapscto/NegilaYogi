using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.TT
{
    public class CLGManualperiodinsertionDelegate
    {
        CommonDelegate<CLGManualperiodinsertionDTO, CLGManualperiodinsertionDTO> comm = new CommonDelegate<CLGManualperiodinsertionDTO, CLGManualperiodinsertionDTO>();

        public CLGManualperiodinsertionDTO getdetails(CLGManualperiodinsertionDTO data)
        {
            return comm.POSTDataTimeTable(data, "CLGManualperiodinsertionFacade/getdetails");
        }
        public CLGManualperiodinsertionDTO get_catg(CLGManualperiodinsertionDTO data)
        {
            return comm.POSTDataTimeTable(data, "CLGManualperiodinsertionFacade/get_catg");
        }
        public CLGManualperiodinsertionDTO getclass_catg(CLGManualperiodinsertionDTO data)
        {
            return comm.POSTDataTimeTable(data, "CLGManualperiodinsertionFacade/getclass_catg");
        }
        public CLGManualperiodinsertionDTO getpossiblePeriod(CLGManualperiodinsertionDTO data)
        {
            return comm.POSTDataTimeTable(data, "CLGManualperiodinsertionFacade/getpossiblePeriod");
        }
        public CLGManualperiodinsertionDTO getrpt(CLGManualperiodinsertionDTO data)
        {
            return comm.POSTDataTimeTable(data, "CLGManualperiodinsertionFacade/getrpt");
        }
        public CLGManualperiodinsertionDTO savedetail(CLGManualperiodinsertionDTO data)
        {
            return comm.POSTDataTimeTable(data, "CLGManualperiodinsertionFacade/savedetail");
        }

       
    }
}
