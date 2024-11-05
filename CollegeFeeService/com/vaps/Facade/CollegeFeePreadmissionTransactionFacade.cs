using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegeFeeService.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Fees;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeFeeService.com.vaps.Facade
{
    [Route("api/[controller]")]
    public class CollegeFeePreadmissionTransactionFacade : Controller
    {
        public CollegeFeePreadmissionTransactionInterface _org;

        public CollegeFeePreadmissionTransactionFacade(CollegeFeePreadmissionTransactionInterface orga)
        {
            _org = orga;
        }

        [HttpPost]
        [Route("getalldetails")]
        public CollegeFeeTransactionDTO Getdet([FromBody] CollegeFeeTransactionDTO data)
        {
            return _org.getdata(data);
        }

        [Route("selectstudent")]
        public CollegeFeeTransactionDTO selectstud([FromBody] CollegeFeeTransactionDTO data)
        {
            return _org.selectstu(data);
        }

        [Route("getgroupmappedheadsnew_st")]
        public CollegeFeeTransactionDTO selectgrptrm([FromBody] CollegeFeeTransactionDTO data)
        {
            return _org.selectgrouppterm(data);
        }

        [Route("savedata_st")]
        public CollegeFeeTransactionDTO savedata([FromBody] CollegeFeeTransactionDTO data)
        {
            return _org.savedata(data);
        }

        [Route("printreceipt_s")]
        public CollegeFeeTransactionDTO printrec([FromBody] CollegeFeeTransactionDTO data)
        {
            return _org.printrec(data);
        }

        [Route("searchfilter")]
        public CollegeFeeTransactionDTO search([FromBody] CollegeFeeTransactionDTO data)
        {
            return _org.search(data);
        }

        [Route("searching_s")]
        public CollegeFeeTransactionDTO searchfilter([FromBody] CollegeFeeTransactionDTO data)
        {
            return _org.searchfilter(data);
        }

        [Route("get_grp_reptno")]
        public CollegeFeeTransactionDTO recnogen([FromBody] CollegeFeeTransactionDTO data)
        {
            return _org.recnogen(data);
        }

        [Route("Deletedetails_s")]
        public CollegeFeeTransactionDTO deleterec([FromBody] CollegeFeeTransactionDTO data)
        {
            return _org.delrec(data);
        }

        [Route("filterstudent")]
        public CollegeFeeTransactionDTO filstude([FromBody] CollegeFeeTransactionDTO data)
        {
            return _org.filstude(data);
        }
    }
}
