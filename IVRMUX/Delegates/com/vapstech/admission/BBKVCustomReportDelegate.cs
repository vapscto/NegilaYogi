using CommonLibrary;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.admission
{
    public class BBKVCustomReportDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<BBKVCustomReportDTO, BBKVCustomReportDTO> tcreport = new CommonDelegate<BBKVCustomReportDTO, BBKVCustomReportDTO>();
        public BBKVCustomReportDTO getdetails(int id)
        {
            return tcreport.GetDataByIdADM(id, "HHSTCCustomReportFacade/getdetails/");
        }

        public BBKVCustomReportDTO getnameregno(BBKVCustomReportDTO data)
        {
            return tcreport.POSTDataADM(data, "HHSTCCustomReportFacade/getnameregno/");
        }
        public BBKVCustomReportDTO stdnamechange(BBKVCustomReportDTO data)
        {
            return tcreport.POSTDataADM(data, "HHSTCCustomReportFacade/stdnamechange/");
        }
        public BBKVCustomReportDTO onclicktcperortemo(BBKVCustomReportDTO data)
        {
            return tcreport.POSTDataADM(data, "HHSTCCustomReportFacade/onclicktcperortemo/");
        }
        public BBKVCustomReportDTO getTcdetails(BBKVCustomReportDTO data)
        {
            return tcreport.POSTDataADM(data, "BBKVCustomReportFacade/getTcdetails/");
        }
        public BBKVCustomReportDTO getTcdetailsJNS(BBKVCustomReportDTO data)
        {
            return tcreport.POSTDataADM(data, "BBKVCustomReportFacade/getTcdetailsJNS/");
        }
        public BBKVCustomReportDTO get_JSHSTcdetails(BBKVCustomReportDTO data)
        {
            return tcreport.POSTDataADM(data, "BBKVCustomReportFacade/get_JSHSTcdetails/");
        }
    }
}
