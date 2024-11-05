using CommonLibrary;
using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Transport
{
    public class RTODetailsDelegate
    {
        CommonDelegate<RTODetailsDTO, RTODetailsDTO> _com = new CommonDelegate<RTODetailsDTO, RTODetailsDTO>();
        public RTODetailsDTO getdata(int id)
        {
            return _com.GetDataByIdTransport(id, "RTODetailsFacade/getdata/");
        }
        public RTODetailsDTO savedata(RTODetailsDTO data)
        {
            return _com.POSTDataTransport(data, "RTODetailsFacade/savedata/");
        }
        public RTODetailsDTO edit(RTODetailsDTO data)
        {
            return _com.POSTDataTransport(data, "RTODetailsFacade/edit/");
        }
        public RTODetailsDTO deleterecord(RTODetailsDTO data)
        {
            return _com.POSTDataTransport(data, "RTODetailsFacade/deleterecord/");
        }

        
        public RTODetailsDTO Onvahiclechange(RTODetailsDTO data)
        {
            return _com.POSTDataTransport(data, "RTODetailsFacade/Onvahiclechange/");
        }
        
    }
}
