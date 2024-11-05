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
    public class CLGRoomMappingDelegate
    {
        CommonDelegate<CLGRoomMappingDTO, CLGRoomMappingDTO> comm = new CommonDelegate<CLGRoomMappingDTO, CLGRoomMappingDTO>();

        public CLGRoomMappingDTO getdetails(CLGRoomMappingDTO data)
        {
            return comm.POSTDataTimeTable(data, "CLGRoomMappingFacade/getdetails");
        }
        public CLGRoomMappingDTO get_catg(CLGRoomMappingDTO data)
        {
            return comm.POSTDataTimeTable(data, "CLGRoomMappingFacade/get_catg");
        }
        public CLGRoomMappingDTO deactiveY(CLGRoomMappingDTO data)
        {
            return comm.POSTDataTimeTable(data, "CLGRoomMappingFacade/deactiveY");
        }
        public CLGRoomMappingDTO getpossiblePeriod(CLGRoomMappingDTO data)
        {
            return comm.POSTDataTimeTable(data, "CLGRoomMappingFacade/getpossiblePeriod");
        }
        public CLGRoomMappingDTO getdays(CLGRoomMappingDTO data)
        {
            return comm.POSTDataTimeTable(data, "CLGRoomMappingFacade/getdays");
        }
        public CLGRoomMappingDTO savedetail(CLGRoomMappingDTO data)
        {
            return comm.POSTDataTimeTable(data, "CLGRoomMappingFacade/savedetail");
        }
        public CLGRoomMappingDTO editdata(CLGRoomMappingDTO data)
        {
            return comm.POSTDataTimeTable(data, "CLGRoomMappingFacade/editdata");
        }
        public CLGRoomMappingDTO get_roomfacility(CLGRoomMappingDTO data)
        {
            return comm.POSTDataTimeTable(data, "CLGRoomMappingFacade/get_roomfacility");
        }

       
    }
}
