using CommonLibrary;
using PreadmissionDTOs.com.vaps.Hostel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Hostel
{
    public class CLGHostelVacateReportDelegate
    {
        CommonDelegate<CLGHostelVacateReportDTO, CLGHostelVacateReportDTO> comm = new CommonDelegate<CLGHostelVacateReportDTO, CLGHostelVacateReportDTO>();
        public CLGHostelVacateReportDTO loaddata(CLGHostelVacateReportDTO obj)
        {
            return comm.Post_Hostel(obj, "CLGHostelVacateReportFacade/loaddata/");
        }
        public CLGHostelVacateReportDTO get_report(CLGHostelVacateReportDTO data)
        {
            return comm.Post_Hostel(data, "CLGHostelVacateReportFacade/get_report/");
        }
        public CLGHostelVacateReportDTO get_Studentlist(CLGHostelVacateReportDTO data)
        {
            return comm.Post_Hostel(data, "CLGHostelVacateReportFacade/get_Studentlist/");
        }

    }
}
