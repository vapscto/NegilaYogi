using CommonLibrary;
using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Transport
{
    public class CLGBusRoutesDetailsDelegate
    {

        CommonDelegate<CLGBusRoutesDetailsDTO, CLGBusRoutesDetailsDTO> comm = new CommonDelegate<CLGBusRoutesDetailsDTO, CLGBusRoutesDetailsDTO>();
public CLGBusRoutesDetailsDTO loaddata(CLGBusRoutesDetailsDTO data)
        {
            return comm.POSTDataTransport(data, "CLGBusRoutesDetailsFacade/loaddata");
        }
        public CLGBusRoutesDetailsDTO getbranch(CLGBusRoutesDetailsDTO data)
        {
            return comm.POSTDataTransport(data, "CLGBusRoutesDetailsFacade/getbranch");
        } public CLGBusRoutesDetailsDTO getsemester(CLGBusRoutesDetailsDTO data)
        {
            return comm.POSTDataTransport(data, "CLGBusRoutesDetailsFacade/getsemester");
        }

      
        public CLGBusRoutesDetailsDTO getreport(CLGBusRoutesDetailsDTO data)
        {
            return comm.POSTDataTransport(data, "CLGBusRoutesDetailsFacade/getreport");
        }
    }
}
