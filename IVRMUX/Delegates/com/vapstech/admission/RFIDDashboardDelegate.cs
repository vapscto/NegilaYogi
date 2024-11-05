using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using PreadmissionDTOs;

using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.admission;
using CommonLibrary;

namespace corewebapi18072016.Delegates
{
    public class RFIDDashboardDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<RFIDDashboardDTO, RFIDDashboardDTO> COMMM = new CommonDelegate<RFIDDashboardDTO, RFIDDashboardDTO>();
      

        public RFIDDashboardDTO Getdetails(RFIDDashboardDTO id)
        {
            return COMMM.POSTDataADM(id, "RFIDDashboardFacade/Getdetails/");

          
        }
        public RFIDDashboardDTO showstudentGrid(RFIDDashboardDTO id)
        {
            return COMMM.POSTDataADM(id, "RFIDDashboardFacade/showstudentGrid/");

          
        }
        public RFIDDashboardDTO cleardata(RFIDDashboardDTO id)
        {
            return COMMM.POSTDataADM(id, "RFIDDashboardFacade/cleardata/");
          
        }

  

          
 
    }
}
