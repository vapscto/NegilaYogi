using CommonLibrary;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.admission
{
    public class DatewiseAttendanceReportDelegate
    {
        CommonDelegate<DatewiseAttendanceReportDTO, DatewiseAttendanceReportDTO> _report = new CommonDelegate<DatewiseAttendanceReportDTO, DatewiseAttendanceReportDTO>();

        public DatewiseAttendanceReportDTO getdata(DatewiseAttendanceReportDTO data)
        {
            return _report.POSTDataaADM(data, "DatewiseAttendanceReportFacade/getdata/");
        }
        public DatewiseAttendanceReportDTO onchangeyear(DatewiseAttendanceReportDTO data)
        {
            return _report.POSTDataaADM(data, "DatewiseAttendanceReportFacade/onchangeyear/");
        }
        public DatewiseAttendanceReportDTO onchangeclass(DatewiseAttendanceReportDTO data)
        {
            return _report.POSTDataaADM(data, "DatewiseAttendanceReportFacade/onchangeclass/");
        }
        public DatewiseAttendanceReportDTO getreport(DatewiseAttendanceReportDTO data)
        {
            return _report.POSTDataaADM(data, "DatewiseAttendanceReportFacade/getreport/");
        }
        public DatewiseAttendanceReportDTO getcountreport(DatewiseAttendanceReportDTO data)
        {
            return _report.POSTDataaADM(data, "DatewiseAttendanceReportFacade/getcountreport/");
        }
        public DatewiseAttendanceReportDTO Reportnew(DatewiseAttendanceReportDTO data)
        {
            return _report.POSTDataaADM(data, "DatewiseAttendanceReportFacade/Reportnew/");
        }
        

    }
}
