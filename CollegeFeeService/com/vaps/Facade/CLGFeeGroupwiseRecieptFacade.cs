using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Fees;
using CollegeFeeService.com.vaps.interfaces;
using PreadmissionDTOs.com.vaps.College.Fees;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeFeeService.com.vaps.Facade
{
    [Route("api/[controller]")]
    public class CLGFeeGroupwiseRecieptFacade : Controller
    {
        public CLGFeeGroupwiseRecieptInterfaces _org;

        public CLGFeeGroupwiseRecieptFacade(CLGFeeGroupwiseRecieptInterfaces orga)
        {
            _org = orga;
        }

        [HttpPost]
        [Route("getalldetails")]
        public CLGFeeGroupwiseRecieptDTO Getdet([FromBody] CLGFeeGroupwiseRecieptDTO data)
        {
            return _org.Getinitialformload(data);
        }

        [HttpPost]
        [Route("getcoursedetails")]
        public CLGFeeGroupwiseRecieptDTO getcourse([FromBody] CLGFeeGroupwiseRecieptDTO data)
        {
            return _org.getcourdetails(data);
        }
        [HttpPost]
        [Route("onsemselection")]
        public CLGFeeGroupwiseRecieptDTO onsemselection([FromBody] CLGFeeGroupwiseRecieptDTO data)
        {
            return _org.onsemselection(data);
        }
        [HttpPost]
        [Route("onselectsec")]
        public CLGFeeGroupwiseRecieptDTO onselectsec([FromBody] CLGFeeGroupwiseRecieptDTO data)
        {
            return _org.onselectsec(data);
        }

        [HttpPost]
        [Route("getbranchdetails")]
        public CLGFeeGroupwiseRecieptDTO getbranch([FromBody] CLGFeeGroupwiseRecieptDTO data)
        {
            return _org.getbranchdetails(data);
        }

        [HttpPost]
        [Route("getsemesterdetails")]
        public CLGFeeGroupwiseRecieptDTO getsemester([FromBody] CLGFeeGroupwiseRecieptDTO data)
        {
            return _org.getsemesterdetails(data);
        }

        [HttpPost]
        [Route("getreceiptreport")]
        public CLGFeeGroupwiseRecieptDTO getreceiptreport([FromBody] CLGFeeGroupwiseRecieptDTO data)
        {
            return _org.getreceiptreport(data);
        }

        [HttpPost]
        [Route("getreceipt")]
        public CLGFeeGroupwiseRecieptDTO getreceipt([FromBody] CLGFeeGroupwiseRecieptDTO data)
        {
            return _org.getreceipt(data);
        }

        

    }
}
