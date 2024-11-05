using CommonLibrary;
using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Transport
{
    public class DriverChartReportDelegate
    {
        CommonDelegate<DriverChartReportDTO, DriverChartReportDTO> _com = new CommonDelegate<DriverChartReportDTO, DriverChartReportDTO>();

        public DriverChartReportDTO getdata(int id)
        {
            return _com.GetDataByIdTransport(id, "DriverChartReportFacade/getdata/");
        }
    
        public DriverChartReportDTO Getreportdetails(DriverChartReportDTO data)
        {
            return _com.POSTDataTransport(data, "DriverChartReportFacade/Getreportdetails/");
        }
        public DriverChartReportDTO vehicletypechange(DriverChartReportDTO data)
        {
            return _com.POSTDataTransport(data, "DriverChartReportFacade/vehicletypechange/");
        }
     
    }
}
