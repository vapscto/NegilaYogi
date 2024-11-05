using CommonLibrary;
using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.FrontOffice
{
    public class StudentInOutReportDelegate
    {
        CommonDelegate<StudentInOutReportDTO, StudentInOutReportDTO> _comm = new CommonDelegate<StudentInOutReportDTO, StudentInOutReportDTO>();
        public StudentInOutReportDTO loaddata(StudentInOutReportDTO data)
        {
            return _comm.POSTDataHolidayReport(data, "StudentInOutReportFacade/loaddata/");
        }
        //getsection
        public StudentInOutReportDTO getsection(StudentInOutReportDTO data)
        {
            return _comm.POSTDataHolidayReport(data, "StudentInOutReportFacade/getsection/");
        }
        //getstudent
        public StudentInOutReportDTO getstudent(StudentInOutReportDTO data)
        {
            return _comm.POSTDataHolidayReport(data, "StudentInOutReportFacade/getstudent/");
        }
        //report
        public StudentInOutReportDTO report(StudentInOutReportDTO data)
        {
            return _comm.POSTDataHolidayReport(data, "StudentInOutReportFacade/report/");
        }
    }
}
