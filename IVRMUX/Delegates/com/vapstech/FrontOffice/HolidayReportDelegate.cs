using CommonLibrary;
using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.FrontOffice
{
    public class HolidayReportDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<MasterHolidayDTO, MasterHolidayDTO> COMFRNT = new CommonDelegate<MasterHolidayDTO, MasterHolidayDTO>();
        public MasterHolidayDTO getdata(int id)
        {
            return COMFRNT.GetDataByIdFROFF(id, "HolidayReportFacade/getdata/");
        }
        public MasterHolidayDTO HolidayReport(MasterHolidayDTO report)
        {

            return COMFRNT.POSTDataHolidayReport(report, "HolidayReportFacade/Report/");

        }
    }
}
