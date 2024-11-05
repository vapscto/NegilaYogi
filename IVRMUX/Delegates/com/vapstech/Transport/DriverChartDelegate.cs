using CommonLibrary;
using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Transport
{
    public class DriverChartDelegate
    {
        CommonDelegate<DriverChartDTO, DriverChartDTO> _com = new CommonDelegate<DriverChartDTO, DriverChartDTO>();
        public DriverChartDTO getdata(int id)
        {
            return _com.GetDataByIdTransport(id, "DriverChartFacade/getdata/");
        }
        public DriverChartDTO savedata(DriverChartDTO data)
        {
            return _com.POSTDataTransport(data, "DriverChartFacade/savedata/");
        }
        public DriverChartDTO edit(DriverChartDTO data)
        {
            return _com.POSTDataTransport(data, "DriverChartFacade/edit/");
        }
        public DriverChartDTO Onvahiclechange(DriverChartDTO data)
        {
            return _com.POSTDataTransport(data, "DriverChartFacade/Onvahiclechange/");
        }
        public DriverChartDTO deleterecord(DriverChartDTO data)
        {
            return _com.POSTDataTransport(data, "DriverChartFacade/deleterecord/");
        }

        
    }
}
