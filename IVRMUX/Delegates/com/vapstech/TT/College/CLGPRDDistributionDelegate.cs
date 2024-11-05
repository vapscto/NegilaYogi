using Newtonsoft.Json;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CommonLibrary;

namespace corewebapi18072016.Delegates.com.vapstech.TT.College
{
    public class CLGPRDDistributionDelegate
    {
        CommonDelegate<CLGPRDDistributionDTO, CLGPRDDistributionDTO> _comm = new CommonDelegate<CLGPRDDistributionDTO, CLGPRDDistributionDTO>();
        
        public CLGPRDDistributionDTO savedetails(CLGPRDDistributionDTO data)
        {
            return _comm.POSTDataTimeTable(data, " CLGPRDDistributionFacade/savedetails/");
        }
        public CLGPRDDistributionDTO getBranch(CLGPRDDistributionDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGPRDDistributionFacade/getBranch/");
        }
        public CLGPRDDistributionDTO savedetail(CLGPRDDistributionDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGPRDDistributionFacade/savedetail/");
        }
        public CLGPRDDistributionDTO getalldetails(CLGPRDDistributionDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGPRDDistributionFacade/getalldetails/");
        }
        public CLGPRDDistributionDTO viewperiods(CLGPRDDistributionDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGPRDDistributionFacade/viewperiods/");
        }
        public CLGPRDDistributionDTO deactivate(CLGPRDDistributionDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGPRDDistributionFacade/deactivate/");
        }
        public CLGPRDDistributionDTO deactivecrsday(CLGPRDDistributionDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGPRDDistributionFacade/deactivecrsday/");
        }
        public CLGPRDDistributionDTO editprddestr(CLGPRDDistributionDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGPRDDistributionFacade/editprddestr/");
        }

    }
}
