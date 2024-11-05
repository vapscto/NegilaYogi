using CommonLibrary;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.admission
{
    public class SwimmingAttendanceReportDelegate
    {
        CommonDelegate<SwimmingAttendanceReportDTO, SwimmingAttendanceReportDTO> _comm = new CommonDelegate<SwimmingAttendanceReportDTO, SwimmingAttendanceReportDTO>();

        public SwimmingAttendanceReportDTO loaddata(SwimmingAttendanceReportDTO data)
        {
            return _comm.POSTDataADM(data, "SwimmingAttendanceReportFacade/loaddata");
        }
        public SwimmingAttendanceReportDTO onchnageyear(SwimmingAttendanceReportDTO data)
        {
            return _comm.POSTDataADM(data, "SwimmingAttendanceReportFacade/onchnageyear");
        }
        public SwimmingAttendanceReportDTO onchangeclass(SwimmingAttendanceReportDTO data)
        {
            return _comm.POSTDataADM(data, "SwimmingAttendanceReportFacade/onchangeclass");
        }
        public SwimmingAttendanceReportDTO search(SwimmingAttendanceReportDTO data)
        {
            return _comm.POSTDataADM(data, "SwimmingAttendanceReportFacade/search");
        }
    }
}
