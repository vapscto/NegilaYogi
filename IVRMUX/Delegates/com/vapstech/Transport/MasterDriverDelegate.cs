using CommonLibrary;
using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Transport
{
    public class MasterDriverDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<MasterDriverDTO, MasterDriverDTO> _driver = new CommonDelegate<MasterDriverDTO, MasterDriverDTO>();

        public MasterDriverDTO getdata(int id)
        {
            return _driver.GetDataByIdTransport(id, "MasterDriverFacade/getdata/");
        }

        public MasterDriverDTO checkdrivercode(MasterDriverDTO data)
        {
            return _driver.POSTDataTransport(data, "MasterDriverFacade/checkdrivercode/");
        }
        public MasterDriverDTO checkdriverdl(MasterDriverDTO data)
        {
            return _driver.POSTDataTransport(data, "MasterDriverFacade/checkdriverdl/");
        }
        public MasterDriverDTO checkdriverbno(MasterDriverDTO data)
        {
            return _driver.POSTDataTransport(data, "MasterDriverFacade/checkdriverbno/");
        }
        public MasterDriverDTO savedata (MasterDriverDTO data)
        {
            return _driver.POSTDataTransport(data, "MasterDriverFacade/savedata/");
        }
        public MasterDriverDTO editdata(MasterDriverDTO data)
        {
            return _driver.POSTDataTransport(data, "MasterDriverFacade/editdata/");
        }
        public MasterDriverDTO activedeactive(MasterDriverDTO data)
        {
            return _driver.POSTDataTransport(data, "MasterDriverFacade/activedeactive/");
        }
        
    }
}
