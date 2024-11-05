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
    public class ColStudentConcessionReportFacade : Controller
    {
        //public AmountEntryReportInterface _feegrouppagee;

        //public AmountEntryReportFacade(AmountEntryReportInterface maspag)
        //{
        //    _feegrouppagee = maspag;
        //}


        //[Route("getdetails")]
        //public CollegeConcessionDTO getorgdet([FromBody]CollegeConcessionDTO dt)
        //{
        //    return _feegrouppagee.getdetails(dt);
        //}

        public ColStudentConcessionReportInterface _feegrouppagee;

        public ColStudentConcessionReportFacade(ColStudentConcessionReportInterface maspag)
        {

            _feegrouppagee = maspag;
        }

        [Route("getalldetails")]
        public CollegeConcessionDTO getalldetails([FromBody] CollegeConcessionDTO data)
        {
            return _feegrouppagee.getalldetails(data);
        }



    }
}
