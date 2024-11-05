using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransportServiceHub.Interfaces
{
    public interface DriverChartReportInterface
    {
        DriverChartReportDTO getdata(int id);
       
     
        DriverChartReportDTO Getreportdetails(DriverChartReportDTO data);
        DriverChartReportDTO vehicletypechange(DriverChartReportDTO data);
      

        
    }
}
