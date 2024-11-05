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
    public class CLGLabDelegate
    {
        CommonDelegate<CLGLabDTO, CLGLabDTO> _comm = new CommonDelegate<CLGLabDTO, CLGLabDTO>();
        
        public CLGLabDTO savedetails(CLGLabDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGLabFacade/savedetails/");
        }
        
        public CLGLabDTO savedetail(CLGLabDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGLabFacade/savedetail/");
        }
        public CLGLabDTO viewrecordspopup(CLGLabDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGLabFacade/viewrecordspopup/");
        }
        public CLGLabDTO getalldetails(CLGLabDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGLabFacade/getalldetails/");
        }
      
        public CLGLabDTO deactivate(CLGLabDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGLabFacade/deactivate/");
        }
        public CLGLabDTO editlab(CLGLabDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGLabFacade/editlab/");
        }
    

    }
}
