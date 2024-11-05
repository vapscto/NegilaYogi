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
    public class CLGMasterDayDelegate
    {
        CommonDelegate<CLGMasterDayDTO, CLGMasterDayDTO> _comm = new CommonDelegate<CLGMasterDayDTO, CLGMasterDayDTO>();
        
        public CLGMasterDayDTO savedetails(CLGMasterDayDTO data)
        {
            return _comm.POSTDataTimeTable(data, " CLGMasterDayFacade/savedetails/");
        }
        public CLGMasterDayDTO getBranch(CLGMasterDayDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGMasterDayFacade/getBranch/");
        }
        public CLGMasterDayDTO saveday(CLGMasterDayDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGMasterDayFacade/saveday/");
        }
        public CLGMasterDayDTO getalldetails(CLGMasterDayDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGMasterDayFacade/getalldetails/");
        }
        public CLGMasterDayDTO editDay(CLGMasterDayDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGMasterDayFacade/editDay/");
        }
        public CLGMasterDayDTO daydeactive(CLGMasterDayDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGMasterDayFacade/daydeactive/");
        }
        public CLGMasterDayDTO deactivecrsday(CLGMasterDayDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGMasterDayFacade/deactivecrsday/");
        }
        public CLGMasterDayDTO savesemday(CLGMasterDayDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGMasterDayFacade/savesemday/");
        }
        public CLGMasterDayDTO getorder(CLGMasterDayDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGMasterDayFacade/getorder/");
        }
        public CLGMasterDayDTO saveorder(CLGMasterDayDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGMasterDayFacade/saveorder/");
        }

    }
}
