using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.admission
{
    public class studenttccustomreportDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<studenttccustomreportDTO, studenttccustomreportDTO> tcreport = new CommonDelegate<studenttccustomreportDTO, studenttccustomreportDTO>();
        public studenttccustomreportDTO getinitialdata(int id)
        {
            return tcreport.GetDataByIdADM(id, "studenttccustomreportFacade/getinitialdata/");
        }
        public studenttccustomreportDTO changeyear(studenttccustomreportDTO data)
        {
            return tcreport.POSTDataADM(data, "studenttccustomreportFacade/changeyear/");
        }
        public studenttccustomreportDTO changeclass(studenttccustomreportDTO data)
        {
            return tcreport.POSTDataADM(data, "studenttccustomreportFacade/changeclass/");
        }
        public studenttccustomreportDTO changesection(studenttccustomreportDTO data)
        {
            return tcreport.POSTDataADM(data, "studenttccustomreportFacade/changesection/");
        }
        public studenttccustomreportDTO getTCdata(studenttccustomreportDTO data)
        {
            return tcreport.POSTDataADM(data, "studenttccustomreportFacade/getTCdata/");
        }
    }
}
