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
    public class SportHouseCommitteeReportFacade : Controller
    {
        public SportHouseCommitteeReportInterface _ReportContext;

        public SportHouseCommitteeReportFacade(SportHouseCommitteeReportInterface dt)
        {
            _ReportContext = dt;
        }


        [Route("Getdetails")]
        public House_Committe_Report_DTO Getdetails([FromBody]House_Committe_Report_DTO data)//int IVRMM_Id
        {

            return _ReportContext.Getdetails(data);
        }

        [Route("showdetails")]
        public Task<House_Committe_Report_DTO> showdetails([FromBody] House_Committe_Report_DTO data)
        {
            return _ReportContext.showdetailsAsync(data);
        }

        [Route("get_House")]
        public House_Committe_Report_DTO get_House([FromBody] House_Committe_Report_DTO data)
        {
            return _ReportContext.get_House(data);
        }

        
    }
}
