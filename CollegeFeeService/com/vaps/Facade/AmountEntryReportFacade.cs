using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CollegeFeeService.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.College.Fees;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeFeeService.com.vaps.Facade
{
    [Route("api/[controller]")]
    public class AmountEntryReportFacade : Controller
    {
        public AmountEntryReportInterface _feegrouppagee;

        public AmountEntryReportFacade(AmountEntryReportInterface maspag)
        {
            _feegrouppagee = maspag;
        }


        [Route("getdetails")]
        public CollegeConcessionDTO getorgdet([FromBody]CollegeConcessionDTO dt)
        {
            return _feegrouppagee.getdetails(dt);
        }

        [HttpPost]
        [Route("get_courses")]
        public CollegeConcessionDTO get_courses([FromBody] CollegeConcessionDTO data)
        {
            return _feegrouppagee.get_courses(data);
        }
        [HttpPost]
        [Route("get_branches")]
        public CollegeConcessionDTO get_branches([FromBody] CollegeConcessionDTO data)
        {
            return _feegrouppagee.get_branches(data);
        }
        [HttpPost]
        [Route("radiobtndata")]
        public CollegeConcessionDTO radiobtndata([FromBody] CollegeConcessionDTO data)
        {
            return _feegrouppagee.radiobtndata(data);
        }
    }
}
