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
    public class CLGStaffReplacementInSectionDelegate
    {

        CommonDelegate<CLGStaffReplacementInSectionDTO, CLGStaffReplacementInSectionDTO> _comm = new CommonDelegate<CLGStaffReplacementInSectionDTO, CLGStaffReplacementInSectionDTO>();

        public CLGStaffReplacementInSectionDTO getdetails(CLGStaffReplacementInSectionDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGStaffReplacementInSectionFacade/getdetails/");
        }
         public CLGStaffReplacementInSectionDTO get_catg(CLGStaffReplacementInSectionDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGStaffReplacementInSectionFacade/get_catg/");
        }
          public CLGStaffReplacementInSectionDTO getclass_catg(CLGStaffReplacementInSectionDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGStaffReplacementInSectionFacade/getclass_catg/");
        }
           public CLGStaffReplacementInSectionDTO getpossiblePeriod(CLGStaffReplacementInSectionDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGStaffReplacementInSectionFacade/getpossiblePeriod/");
        }
           public CLGStaffReplacementInSectionDTO getrpt(CLGStaffReplacementInSectionDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGStaffReplacementInSectionFacade/getrpt/");
        }

         public CLGStaffReplacementInSectionDTO savedetail(CLGStaffReplacementInSectionDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGStaffReplacementInSectionFacade/savedetail/");
        }
        
    }
}
