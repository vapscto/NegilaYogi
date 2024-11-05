using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Sports;
using SportsServiceHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SportsServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class SportHouseReportFacade : Controller
    {
        public SportHouseReportInterface _ReportContext;

        public SportHouseReportFacade(SportHouseReportInterface dt)
        {
            _ReportContext = dt;
        }


        [Route("Getdetails")]
        public House_Report_DTO Getdetails([FromBody]House_Report_DTO data)//int IVRMM_Id
        {

            return _ReportContext.Getdetails(data);
        }


        [Route("showdetails")]
        public Task<House_Report_DTO> showdetails([FromBody] House_Report_DTO data)
        {
            return _ReportContext.showdetails(data);
        }
        //showdetailsNew
        [Route("showdetailsNew")]
        public Task<House_Report_DTO> showdetailsNew([FromBody] House_Report_DTO data)
        {
            return _ReportContext.showdetailsNew(data);
        }
        [Route("get_class")]
        public House_Report_DTO get_class([FromBody] House_Report_DTO data)
        {
            return _ReportContext.get_class(data);
        }
        [Route("get_section")]
        public House_Report_DTO get_section([FromBody] House_Report_DTO data)
        {
            return _ReportContext.get_section(data);
        }
       
    }
}
