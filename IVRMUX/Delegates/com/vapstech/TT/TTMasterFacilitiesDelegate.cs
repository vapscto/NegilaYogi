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
    public class TTMasterFacilitiesDelegate
    {

        CommonDelegate<TTMasterFacilitiesDTO, TTMasterFacilitiesDTO> _comm = new CommonDelegate<TTMasterFacilitiesDTO, TTMasterFacilitiesDTO>();

        public TTMasterFacilitiesDTO savedetail(TTMasterFacilitiesDTO data)
        {
            return _comm.POSTDataTimeTable(data, "TTMasterFacilitiesFacade/savedetail/");
        }

        public TTMasterFacilitiesDTO getdetails(int id)
        {
            return _comm.GetDataByIdTimeTable(id, "TTMasterFacilitiesFacade/getdetails/");
        }
        public TTMasterFacilitiesDTO getpagedetails(int id)
        {
            return _comm.GetDataByIdTimeTable(id, "TTMasterFacilitiesFacade/getpagedetails/");
        }
        



        public TTMasterFacilitiesDTO deactivate(TTMasterFacilitiesDTO data)
        {
            return _comm.POSTDataTimeTable(data, "TTMasterFacilitiesFacade/deactivate/");
        }




       

    }
}
