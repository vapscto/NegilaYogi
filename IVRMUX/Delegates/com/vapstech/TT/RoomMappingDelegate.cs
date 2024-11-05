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
    public class RoomMappingDelegate
    {
        CommonDelegate<RoomMappingDTO, RoomMappingDTO> comm = new CommonDelegate<RoomMappingDTO, RoomMappingDTO>();

        public RoomMappingDTO getdetails(RoomMappingDTO data)
        {
            return comm.POSTDataTimeTable(data, "RoomMappingFacade/getdetails");
        }
        public RoomMappingDTO get_catg(RoomMappingDTO data)
        {
            return comm.POSTDataTimeTable(data, "RoomMappingFacade/get_catg");
        }
        public RoomMappingDTO deactiveY(RoomMappingDTO data)
        {
            return comm.POSTDataTimeTable(data, "RoomMappingFacade/deactiveY");
        }
        public RoomMappingDTO getpossiblePeriod(RoomMappingDTO data)
        {
            return comm.POSTDataTimeTable(data, "RoomMappingFacade/getpossiblePeriod");
        }
        public RoomMappingDTO getrpt(RoomMappingDTO data)
        {
            return comm.POSTDataTimeTable(data, "RoomMappingFacade/getrpt");
        }
        public RoomMappingDTO savedetail(RoomMappingDTO data)
        {
            return comm.POSTDataTimeTable(data, "RoomMappingFacade/savedetail");
        }
        public RoomMappingDTO editdata(RoomMappingDTO data)
        {
            return comm.POSTDataTimeTable(data, "RoomMappingFacade/editdata");
        }

       
    }
}
