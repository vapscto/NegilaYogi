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
    public class CLGStaffPeriodTransformDelegate
    {
        CommonDelegate<CLGStaffPeriodTransformDTO, CLGStaffPeriodTransformDTO> _comm = new CommonDelegate<CLGStaffPeriodTransformDTO, CLGStaffPeriodTransformDTO>();

        public CLGStaffPeriodTransformDTO getdetails(CLGStaffPeriodTransformDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGStaffPeriodTransformFacade/getdetails/");
        }
        public CLGStaffPeriodTransformDTO get_catg(CLGStaffPeriodTransformDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGStaffPeriodTransformFacade/get_catg/");
        }
        public CLGStaffPeriodTransformDTO getrpt(CLGStaffPeriodTransformDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGStaffPeriodTransformFacade/getrpt/");
        }
        public CLGStaffPeriodTransformDTO gettimetable(CLGStaffPeriodTransformDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGStaffPeriodTransformFacade/gettimetable/");
        }
        public CLGStaffPeriodTransformDTO getpossiblePeriod(CLGStaffPeriodTransformDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGStaffPeriodTransformFacade/getpossiblePeriod/");
        }
        public CLGStaffPeriodTransformDTO savedetail(CLGStaffPeriodTransformDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGStaffPeriodTransformFacade/savedetail/");
        }
        public CLGStaffPeriodTransformDTO deleteperiod(CLGStaffPeriodTransformDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGStaffPeriodTransformFacade/deleteperiod/");
        }
        
    }
}
