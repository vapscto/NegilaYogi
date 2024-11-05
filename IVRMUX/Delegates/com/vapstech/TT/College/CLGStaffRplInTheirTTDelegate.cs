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
    public class CLGStaffRplInTheirTTDelegate
    {
        CommonDelegate<CLGStaffRplInTheirTTDTO, CLGStaffRplInTheirTTDTO> _comm = new CommonDelegate<CLGStaffRplInTheirTTDTO, CLGStaffRplInTheirTTDTO>();

        public CLGStaffRplInTheirTTDTO getdetails(CLGStaffRplInTheirTTDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGStaffRplInTheirTTFacade/getdetails/");
        }
        public CLGStaffRplInTheirTTDTO get_catg(CLGStaffRplInTheirTTDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGStaffRplInTheirTTFacade/get_catg/");
        }
        public CLGStaffRplInTheirTTDTO getrpt(CLGStaffRplInTheirTTDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGStaffRplInTheirTTFacade/getrpt/");
        }
        public CLGStaffRplInTheirTTDTO getpossiblePeriod(CLGStaffRplInTheirTTDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGStaffRplInTheirTTFacade/getpossiblePeriod/");
        }
        public CLGStaffRplInTheirTTDTO savedetail(CLGStaffRplInTheirTTDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGStaffRplInTheirTTFacade/savedetail/");
        }
       
       
    }
}
