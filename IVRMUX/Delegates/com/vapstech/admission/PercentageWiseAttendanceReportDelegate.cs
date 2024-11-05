using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.admission;

namespace IVRMUX.Delegates.com.vapstech.admission
{
    public class PercentageWiseAttendanceReportDelegate
    {
        CommonDelegate<PercentageWiseAttendanceReportDTO, PercentageWiseAttendanceReportDTO> _comm = new CommonDelegate<PercentageWiseAttendanceReportDTO, PercentageWiseAttendanceReportDTO>();

        public PercentageWiseAttendanceReportDTO getloaddata (PercentageWiseAttendanceReportDTO data)
        {
            return _comm.POSTDataADM(data, "PercentageWiseAttendanceReportFacde/getloaddata");
        }
        public PercentageWiseAttendanceReportDTO getclass(PercentageWiseAttendanceReportDTO data)
        {
            return _comm.POSTDataADM(data, "PercentageWiseAttendanceReportFacde/getclass");
        }
        public PercentageWiseAttendanceReportDTO getsection(PercentageWiseAttendanceReportDTO data)
        {
            return _comm.POSTDataADM(data, "PercentageWiseAttendanceReportFacde/getsection");
        }
        public PercentageWiseAttendanceReportDTO showreport(PercentageWiseAttendanceReportDTO data)
        {
            return _comm.POSTDataADM(data, "PercentageWiseAttendanceReportFacde/showreport");
        }  
        public PercentageWiseAttendanceReportDTO SendAttendanceSMS(PercentageWiseAttendanceReportDTO data)
        {
            return _comm.POSTDataADM(data, "PercentageWiseAttendanceReportFacde/SendAttendanceSMS");
        }    
    }
}
