using CommonLibrary;
using PreadmissionDTOs.com.vaps.Hostel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Hostel
{
    public class Hostel_Request_ReportDelegate
    {
        CommonDelegate<Hostel_Request_ReportDTO, Hostel_Request_ReportDTO> _comm = new CommonDelegate<Hostel_Request_ReportDTO, Hostel_Request_ReportDTO>();

        public Hostel_Request_ReportDTO getdata(Hostel_Request_ReportDTO data)
        {
            return _comm.Post_Hostel(data, "Hostel_Request_ReportFacade/getdata/");
        }
        public Hostel_Request_ReportDTO getreport(Hostel_Request_ReportDTO data)
        {
            return _comm.Post_Hostel(data, "Hostel_Request_ReportFacade/getreport/");
        }
        public Hostel_Request_ReportDTO getconfirmreport(Hostel_Request_ReportDTO data)
        {
            return _comm.Post_Hostel(data, "Hostel_Request_ReportFacade/getconfirmreport/");
        }
    }
}
