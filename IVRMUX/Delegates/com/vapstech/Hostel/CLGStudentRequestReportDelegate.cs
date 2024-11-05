using CommonLibrary;
using PreadmissionDTOs.com.vaps.Hostel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Hostel
{
    public class CLGStudentRequestReportDelegate
    {
        CommonDelegate<CLGStudentReportDTO, CLGStudentReportDTO> _comm = new CommonDelegate<CLGStudentReportDTO, CLGStudentReportDTO>();

        public CLGStudentReportDTO getdata(CLGStudentReportDTO data)
        {
            return _comm.Post_Hostel(data, "CLGStudentRequestReportFacade/getdata/");
        }
        public CLGStudentReportDTO getreport(CLGStudentReportDTO data)
        {
            return _comm.Post_Hostel(data, "CLGStudentRequestReportFacade/getreport/");
        }
        public CLGStudentReportDTO getconfirmreport(CLGStudentReportDTO data)
        {
            return _comm.Post_Hostel(data, "CLGStudentRequestReportFacade/getconfirmreport/");
        }
    }
}

