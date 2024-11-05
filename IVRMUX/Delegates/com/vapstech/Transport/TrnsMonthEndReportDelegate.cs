using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.Transport;

namespace corewebapi18072016.Delegates.com.vapstech.Transport
{
    public class TrnsMonthEndReportDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<TrnsMonthEndReportDTO, TrnsMonthEndReportDTO> _areazone = new CommonDelegate<TrnsMonthEndReportDTO, TrnsMonthEndReportDTO>();

        public TrnsMonthEndReportDTO getdata1(int id)
        {
            return _areazone.GetDataByIdTransport(id, "TrnsMonthEndReportFacade/getdata1/");
        }
        public TrnsMonthEndReportDTO savedata1(TrnsMonthEndReportDTO data)
        {
            return _areazone.POSTDataTransport(data, "TrnsMonthEndReportFacade/savedata1/");
        }

        public TrnsMonthEndReportDTO getdata(int id)
        {
            return _areazone.GetDataByIdTransport(id, "TrnsMonthEndReportFacade/getdata/");
        }
        public TrnsMonthEndReportDTO savedata(TrnsMonthEndReportDTO data)
        {
            return _areazone.POSTDataTransport(data, "TrnsMonthEndReportFacade/savedata/");
        }
        public TrnsMonthEndReportDTO geteditdata(TrnsMonthEndReportDTO data)
        {
            return _areazone.POSTDataTransport(data, "TrnsMonthEndReportFacade/geteditdata/");
        }
        public TrnsMonthEndReportDTO activedeactive(TrnsMonthEndReportDTO data)
        {
            return _areazone.POSTDataTransport(data, "TrnsMonthEndReportFacade/activedeactive/");
        }
    }

}
