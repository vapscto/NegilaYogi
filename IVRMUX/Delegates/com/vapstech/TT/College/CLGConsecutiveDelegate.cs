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
    public class CLGConsecutiveDelegate
    {
        CommonDelegate<CLGConsecutiveDTO, CLGConsecutiveDTO> _comm = new CommonDelegate<CLGConsecutiveDTO, CLGConsecutiveDTO>();
        
        public CLGConsecutiveDTO savedetails(CLGConsecutiveDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGConsecutiveFacade/savedetails/");
        }
        
        public CLGConsecutiveDTO savedetail(CLGConsecutiveDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGConsecutiveFacade/savedetail/");
        }
        public CLGConsecutiveDTO getalldetails(CLGConsecutiveDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGConsecutiveFacade/getalldetails/");
        }
      
        public CLGConsecutiveDTO deactivate(CLGConsecutiveDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGConsecutiveFacade/deactivate/");
        }
        public CLGConsecutiveDTO editconv(CLGConsecutiveDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGConsecutiveFacade/editconv/");
        }
    

    }
}
