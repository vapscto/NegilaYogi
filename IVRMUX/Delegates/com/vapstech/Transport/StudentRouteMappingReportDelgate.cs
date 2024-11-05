using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using Newtonsoft.Json;

namespace corewebapi18072016.Delegates.com.vapstech.Transport
{
    public class StudentRouteMappingReportDelgate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<StudentRouteMappingReportDTO, StudentRouteMappingReportDTO> comml = new CommonDelegate<StudentRouteMappingReportDTO, StudentRouteMappingReportDTO>();

        public StudentRouteMappingReportDTO Getreportdetails(StudentRouteMappingReportDTO data)
        {
            return comml.POSTDataTransport(data, "StudentRouteMappingReportFacade/Getreportdetails/");
        }
        public StudentRouteMappingReportDTO getdata(int id)
        {
            return comml.GetDataByIdTransport(id, "StudentRouteMappingReportFacade/getdata/");
        }
    }
}
