using CommonLibrary;
using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Sports
{
    public class SRKVSSportsReportDelagte
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<SRKVSSportsReportDTO, SRKVSSportsReportDTO> COMMM = new CommonDelegate<SRKVSSportsReportDTO, SRKVSSportsReportDTO>();


        public SRKVSSportsReportDTO Getdetails(SRKVSSportsReportDTO data)
        {
            return COMMM.POSTDataSports(data, "SRKVSSportsReportFacade/Getdetails/");
        }

        public SRKVSSportsReportDTO showdetails(SRKVSSportsReportDTO data)
        {
            return COMMM.POSTDataSports(data, "SRKVSSportsReportFacade/showdetails/");
        }

        public SRKVSSportsReportDTO get_class(SRKVSSportsReportDTO data)
        {
            return COMMM.POSTDataSports(data, "SRKVSSportsReportFacade/get_class/");
        }
        public SRKVSSportsReportDTO get_classs(SRKVSSportsReportDTO data)
        {
            return COMMM.POSTDataSports(data, "SRKVSSportsReportFacade/get_classs/");
        }
        public SRKVSSportsReportDTO get_section(SRKVSSportsReportDTO data)
        {
            return COMMM.POSTDataSports(data, "SRKVSSportsReportFacade/get_section/");
        }

        public SRKVSSportsReportDTO get_student(SRKVSSportsReportDTO data)
        {
            return COMMM.POSTDataSports(data, "SRKVSSportsReportFacade/get_student/");
        }
    }
}
