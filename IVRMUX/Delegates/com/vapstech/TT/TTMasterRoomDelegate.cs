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
    public class TTMasterRoomDelegate
    {

        CommonDelegate<TTMasterRoomDTO, TTMasterRoomDTO> _comm = new CommonDelegate<TTMasterRoomDTO, TTMasterRoomDTO>();

        public TTMasterRoomDTO savedetail(TTMasterRoomDTO data)
        {
            return _comm.POSTDataTimeTable(data, "TTMasterRoomFacade/savedetail/");
        }

        public TTMasterRoomDTO getdetails(int id)
        {
            return _comm.GetDataByIdTimeTable(id, "TTMasterRoomFacade/getdetails/");
        }
        public TTMasterRoomDTO getpagedetails(int id)
        {
            return _comm.GetDataByIdTimeTable(id, "TTMasterRoomFacade/getpagedetails/");
        }
           public TTMasterRoomDTO Viewfacility(int id)
        {
            return _comm.GetDataByIdTimeTable(id, "TTMasterRoomFacade/Viewfacility/");
        }
        



        public TTMasterRoomDTO deactivate(TTMasterRoomDTO data)
        {
            return _comm.POSTDataTimeTable(data, "TTMasterRoomFacade/deactivate/");
        }




       

    }
}
