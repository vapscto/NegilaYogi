using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FeeServiceHub.com.vaps.interfaces;
using FeeServiceHub.com.vaps.services;
using PreadmissionDTOs.com.vaps.Fees;
using PreadmissionDTOs.com.vaps.admission;
// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class FeeStudentConcessionReportFacadeController : Controller
    {
        public FeeStudentConcessionReportInterface _feetar;

        public FeeStudentConcessionReportFacadeController(FeeStudentConcessionReportInterface maspag)
        {
            _feetar = maspag;
        }

     
      
        [HttpPost]
        [Route("getalldetails123")]
        public StudentConcesstionDTO Getdet([FromBody] StudentConcesstionDTO data)
        {
            return _feetar.getdata123(data);
        }
       
        [Route("getreport")]
        public StudentConcesstionDTO getreport([FromBody] StudentConcesstionDTO data)
        {
            return _feetar.getreport(data);
        }
        [Route("get_groups")]
        public FeeStudentTransactionDTO get_groups([FromBody]FeeStudentTransactionDTO temp)
        {
            return _feetar.get_groups(temp);
        }

    }
}
