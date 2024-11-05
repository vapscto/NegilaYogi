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
    public class HouseInchargeReportFacade : Controller
    {
        // GET: api/<controller>

        HouseInchargeReportInterface _objinter;
        public HouseInchargeReportFacade(HouseInchargeReportInterface obj)
        {
            _objinter = obj;
        }

        [Route("get_details")]
        public HouseInchargeReport_DTO get_details([FromBody] HouseInchargeReport_DTO data)
        {
            return _objinter.get_details(data);
        }


        [Route("get_house")]
        public HouseInchargeReport_DTO get_house([FromBody] HouseInchargeReport_DTO data)
        {
            return _objinter.get_house(data);
        }


        [Route("get_reports")]
        public  Task<HouseInchargeReport_DTO> get_reports([FromBody] HouseInchargeReport_DTO data)
        {
            return _objinter.get_reports(data);
        }

    }
}
