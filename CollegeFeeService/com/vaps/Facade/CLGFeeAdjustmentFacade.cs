using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Fees;
using CollegeFeeService.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeFeeService.com.vaps.Facade
{
    [Route("api/[controller]")]
    public class CLGFeeAdjustmentFacade : Controller
    {
        public CLGFeeAdjustmentInterface _org;
        public CLGFeeAdjustmentFacade(CLGFeeAdjustmentInterface orga)
        {
            _org = orga;
        }

        [HttpPost]
        [Route("getalldetails")]
        public CLGFeeAdjustmentDTO Getdet([FromBody] CLGFeeAdjustmentDTO data)
        {
            return _org.getdata(data);
        }

        [HttpPost]
        [Route("getclass")]
        public CLGFeeAdjustmentDTO Getclass([FromBody]CLGFeeAdjustmentDTO data)
        {
            return _org.getdataclassdet(data);
        }
        [HttpPost]
        [Route("getsection")]
        public CLGFeeAdjustmentDTO Getsection([FromBody]CLGFeeAdjustmentDTO data)
        {
            return _org.getdatasectiondet(data);
        }

        [HttpPost]
        [Route("getstudent")]
        public CLGFeeAdjustmentDTO GetStudent([FromBody]CLGFeeAdjustmentDTO data)
        {
            return _org.getdatastudentdet(data);
        }

        [HttpPost]
        [Route("getbothgroup")]
        public CLGFeeAdjustmentDTO Getbothgroup([FromBody]CLGFeeAdjustmentDTO data)
        {
            return _org.getdatabothgroupdet(data);
        }

        [HttpPost]
        [Route("getfromhead")]
        public CLGFeeAdjustmentDTO Getfromhead([FromBody]CLGFeeAdjustmentDTO data)
        {
            return _org.getdatafromheaddet(data);
        }

        [HttpPost]
        [Route("gettohead")]
        public CLGFeeAdjustmentDTO Gettohead([FromBody]CLGFeeAdjustmentDTO data)
        {
            return _org.getdatatoheaddet(data);
        }

       
        [Route("savedata")]
        public CLGFeeAdjustmentDTO savedataa([FromBody] CLGFeeAdjustmentDTO data)
        {

            return _org.savedatadelegate(data);
        }
        [Route("getpagedetails/{id:int}")]
        public CLGFeeAdjustmentDTO getpagedetails(int id)
        {
            // id = 12;
            return _org.getpageedit(id);
        }
        //[HttpDelete]
        //[Route("deletedetails/{id:int}")]
        //public CLGFeeAdjustmentDTO Deleterec(int id)
        //{
        //    return _org.deleterec(id);
        //}
        //[HttpPost]
        //[Route("deactivate")]
        //public CLGFeeAdjustmentDTO Deactivate([FromBody] CLGFeeAdjustmentDTO id)
        //{
        //    // id = 12;
        //    return _org.deactivate(id);
        //}
        [HttpDelete]
        [Route("deletedetails/{id:int}")]
        public CLGFeeAdjustmentDTO Deleterec(int id)
        {
            return _org.deleterec(id);
        }
        [HttpPost]
        [Route("searching")]
        public CLGFeeAdjustmentDTO searching([FromBody] CLGFeeAdjustmentDTO data)
        {
            return _org.searching(data);
        }
    }
}
