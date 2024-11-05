using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CollegeFeeService.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.College.Fees;
using PreadmissionDTOs.com.vaps.Fees;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeFeeService.com.vaps.Facade
{
    [Route("api/[controller]")]
    public class CollegeDefaultersReportFacade : Controller
    {
        public CollegeDefaultersReportInterface _feegrouppagee;

        public CollegeDefaultersReportFacade(CollegeDefaultersReportInterface maspag)
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
        [Route("get_semisters")]
        public CollegeConcessionDTO get_semisters([FromBody] CollegeConcessionDTO data)
        {
            return _feegrouppagee.get_semisters(data);
        }

        [HttpPost]
        [Route("radiobtndata")]
        public Task<CollegeConcessionDTO> radiobtndata([FromBody] CollegeConcessionDTO data)
        {
            return _feegrouppagee.radiobtndata(data);
        }

        [Route("sendsms")]
        public Task<FeetransactionSMS> sendsms([FromBody]FeetransactionSMS data)
        {
            return _feegrouppagee.sendsms(data);
        }
        [Route("sendemail")]
        public FeetransactionSMS sendemail([FromBody]FeetransactionSMS data)
        {
            return _feegrouppagee.sendemail(data);
        }

        [Route("duedateExcededRemainder")]
        public CollegeConcessionDTO duedateExcededRemainder([FromBody] CollegeConcessionDTO data)
        {
            return _feegrouppagee.duedateExcededRemainder(data);
        }
        [Route("DuedateRemainder")]
        public CollegeConcessionDTO DuedateRemainder([FromBody] CollegeConcessionDTO data)
        {
            return _feegrouppagee.DuedateRemainder(data);
        }

        [Route("duedatesms")]
        public Task<CollegeConcessionDTO> duedatesms([FromBody]CollegeConcessionDTO data)
        {
            return _feegrouppagee.duedatesms(data);
        }
    }
}
