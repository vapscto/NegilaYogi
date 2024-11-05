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
    public class CLGBifurcationDelegate
    {
        CommonDelegate<CLGBifurcationDTO, CLGBifurcationDTO> _comm = new CommonDelegate<CLGBifurcationDTO, CLGBifurcationDTO>();
        
        public CLGBifurcationDTO savedetails(CLGBifurcationDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGBifurcationFacade/savedetails/");
        }
        public CLGBifurcationDTO getBranch(CLGBifurcationDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGBifurcationFacade/getBranch/");
        }
        public CLGBifurcationDTO savedetailBiff(CLGBifurcationDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGBifurcationFacade/savedetailBiff/");
        }
        public CLGBifurcationDTO getalldetails(CLGBifurcationDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGBifurcationFacade/getalldetails/");
        }
        public CLGBifurcationDTO editDay(CLGBifurcationDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGBifurcationFacade/editDay/");
        }
        public CLGBifurcationDTO deactivatebiff(CLGBifurcationDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGBifurcationFacade/deactivatebiff/");
        }
        public CLGBifurcationDTO viewrecordspopup(CLGBifurcationDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGBifurcationFacade/viewrecordspopup/");
        }
        public CLGBifurcationDTO editbiff(CLGBifurcationDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGBifurcationFacade/editbiff/");
        }

    }
}
