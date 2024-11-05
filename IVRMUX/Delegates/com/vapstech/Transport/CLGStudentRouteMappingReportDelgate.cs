using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using Newtonsoft.Json;

namespace corewebapi18072016.Delegates.com.vapstech.Transport
{
    public class CLGStudentRouteMappingReportDelgate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<CLGStudentRouteMappingReportDTO, CLGStudentRouteMappingReportDTO> comml = new CommonDelegate<CLGStudentRouteMappingReportDTO, CLGStudentRouteMappingReportDTO>();

        public CLGStudentRouteMappingReportDTO Getreportdetails(CLGStudentRouteMappingReportDTO data)
        {
            return comml.POSTDataTransport(data, "CLGStudentRouteMappingReportFacade/Getreportdetails/");
        }
        public CLGStudentRouteMappingReportDTO getdata(int id)
        {
            return comml.GetDataByIdTransport(id, "CLGStudentRouteMappingReportFacade/getdata/");
        }
    }
}
