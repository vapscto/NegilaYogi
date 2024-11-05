using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Sports;
using SportsServiceHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SportsServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class SRKVSSportsReportFacade : Controller
    {
        public SRKVSSportsReportInterface _ReportContext;

        public SRKVSSportsReportFacade(SRKVSSportsReportInterface dt)
        {
            _ReportContext = dt;
        }


        [Route("Getdetails")]
        public SRKVSSportsReportDTO Getdetails([FromBody]SRKVSSportsReportDTO data)//int IVRMM_Id
        {

            return _ReportContext.Getdetails(data);
        }


        [Route("showdetails")]
        public SRKVSSportsReportDTO showdetails([FromBody] SRKVSSportsReportDTO data)
        {
            return _ReportContext.showdetails(data);
        }

        [Route("get_class")]
        public SRKVSSportsReportDTO get_class([FromBody] SRKVSSportsReportDTO data)
        {
            return _ReportContext.get_class(data);
        }

        [Route("get_classs")]
        public SRKVSSportsReportDTO get_classs([FromBody] SRKVSSportsReportDTO data)
        {
            return _ReportContext.get_classs(data);
        }

        [Route("get_section")]
        public SRKVSSportsReportDTO get_section([FromBody] SRKVSSportsReportDTO data)
        {
            return _ReportContext.get_section(data);
        }
    }
}
