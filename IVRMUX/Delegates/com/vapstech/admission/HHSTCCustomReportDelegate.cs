using CommonLibrary;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.admission
{
    public class HHSTCCustomReportDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HHSTCCustomReportDTO, HHSTCCustomReportDTO> tcreport = new CommonDelegate<HHSTCCustomReportDTO, HHSTCCustomReportDTO>();
        public HHSTCCustomReportDTO getdetails(int id)
        {
            return tcreport.GetDataByIdADM(id, "HHSTCCustomReportFacade/getdetails/");
        }

        public HHSTCCustomReportDTO getnameregno(HHSTCCustomReportDTO data)
        {
            return tcreport.POSTDataADM(data, "HHSTCCustomReportFacade/getnameregno/");
        }
        public HHSTCCustomReportDTO stdnamechange(HHSTCCustomReportDTO data)
        {
            return tcreport.POSTDataADM(data, "HHSTCCustomReportFacade/stdnamechange/");
        }
        public HHSTCCustomReportDTO onclicktcperortemo(HHSTCCustomReportDTO data)
        {
            return tcreport.POSTDataADM(data, "HHSTCCustomReportFacade/onclicktcperortemo/");
        }
        public HHSTCCustomReportDTO getTcdetails(HHSTCCustomReportDTO data)
        {
            return tcreport.POSTDataADM(data, "HHSTCCustomReportFacade/getTcdetails/");
        }
        public HHSTCCustomReportDTO Vikasha_getTcdetails(HHSTCCustomReportDTO data)
        {
            return tcreport.POSTDataADM(data, "HHSTCCustomReportFacade/Vikasha_getTcdetails/");
        }


    }
}
