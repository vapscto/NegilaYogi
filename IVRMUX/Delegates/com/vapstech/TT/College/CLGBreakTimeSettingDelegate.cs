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
    public class CLGBreakTimeSettingDelegate
    {
        CommonDelegate<CLGBreakTimeSettingDTO, CLGBreakTimeSettingDTO> _comm = new CommonDelegate<CLGBreakTimeSettingDTO, CLGBreakTimeSettingDTO>();
        
        public CLGBreakTimeSettingDTO savedetails(CLGBreakTimeSettingDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGBreakTimeSettingFacade/savedetails/");
        }
        public CLGBreakTimeSettingDTO getBranch(CLGBreakTimeSettingDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGBreakTimeSettingFacade/getBranch/");
        }
        public CLGBreakTimeSettingDTO savetimedetail(CLGBreakTimeSettingDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGBreakTimeSettingFacade/savetimedetail/");
        }
        public CLGBreakTimeSettingDTO getalldetails(CLGBreakTimeSettingDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGBreakTimeSettingFacade/getalldetails/");
        }
        public CLGBreakTimeSettingDTO editDay(CLGBreakTimeSettingDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGBreakTimeSettingFacade/editDay/");
        }
        public CLGBreakTimeSettingDTO deactivate(CLGBreakTimeSettingDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGBreakTimeSettingFacade/deactivate/");
        }
        public CLGBreakTimeSettingDTO geteditdetails(CLGBreakTimeSettingDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGBreakTimeSettingFacade/geteditdetails/");
        }
        public CLGBreakTimeSettingDTO getmaximumperiodscount(CLGBreakTimeSettingDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGBreakTimeSettingFacade/getmaximumperiodscount/");
        }

    }
}
