using CommonLibrary;
using PreadmissionDTOs.com.vaps.Hostel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Hostel
{
    public class HostelVacateReportDelegate
    {
        CommonDelegate<HostelVacateReportDTO, HostelVacateReportDTO> comm = new CommonDelegate<HostelVacateReportDTO, HostelVacateReportDTO>();
        public HostelVacateReportDTO loaddata(HostelVacateReportDTO obj)
        {
            return comm.Post_Hostel(obj, "HostelVacateReportFacade/loaddata/");
        }
        public HostelVacateReportDTO get_report(HostelVacateReportDTO data)
        {
            return comm.Post_Hostel(data, "HostelVacateReportFacade/get_report/");
        }
        public HostelVacateReportDTO get_Studentlist(HostelVacateReportDTO data)
        {
            return comm.Post_Hostel(data, "HostelVacateReportFacade/get_Studentlist/");
        }

    }
}
