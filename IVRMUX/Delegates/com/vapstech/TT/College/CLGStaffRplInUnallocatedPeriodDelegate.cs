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
    public class CLGStaffRplInUnallocatedPeriodDelegate
    {



        CommonDelegate<CLGStaffRplInUnallocatedPeriodDTO, CLGStaffRplInUnallocatedPeriodDTO> _comm = new CommonDelegate<CLGStaffRplInUnallocatedPeriodDTO, CLGStaffRplInUnallocatedPeriodDTO>();

        public CLGStaffRplInUnallocatedPeriodDTO getdetails(CLGStaffRplInUnallocatedPeriodDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGStaffRplInUnallocatedPeriodFacade/getdetails/");
        }
        public CLGStaffRplInUnallocatedPeriodDTO get_catg(CLGStaffRplInUnallocatedPeriodDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGStaffRplInUnallocatedPeriodFacade/get_catg/");
        }
        public CLGStaffRplInUnallocatedPeriodDTO getrpt(CLGStaffRplInUnallocatedPeriodDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGStaffRplInUnallocatedPeriodFacade/getrpt/");
        }
         public CLGStaffRplInUnallocatedPeriodDTO savedetail(CLGStaffRplInUnallocatedPeriodDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGStaffRplInUnallocatedPeriodFacade/savedetail/");
        }
        
        
    }
}
