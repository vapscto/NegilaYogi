using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransportServiceHub.Interfaces
{
    public interface DriverChartInterface
    {
        DriverChartDTO getdata(int id);
        DriverChartDTO savedata(DriverChartDTO data);
        DriverChartDTO edit(DriverChartDTO data);
        DriverChartDTO Onvahiclechange(DriverChartDTO data);
        DriverChartDTO deleterecord(DriverChartDTO data);

        
    }
}
