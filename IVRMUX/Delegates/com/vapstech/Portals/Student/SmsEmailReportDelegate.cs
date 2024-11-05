using CommonLibrary;
using PreadmissionDTOs.com.vaps.Portals.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Portals.Student
{
    public class SmsEmailReportDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<SmsEmailReportDTO, SmsEmailReportDTO> COMMM = new CommonDelegate<SmsEmailReportDTO, SmsEmailReportDTO>();
        public SmsEmailReportDTO getloaddata(SmsEmailReportDTO data)
        {
            return COMMM.POSTPORTALData(data, "SmsEmailReportFacade/getloaddata/");
        }

        public SmsEmailReportDTO getdata(SmsEmailReportDTO sddto)
        {
            return COMMM.POSTPORTALData(sddto, "SmsEmailReportFacade/getdata/");
        }
        
    }
}
