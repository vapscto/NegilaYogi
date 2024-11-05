using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FeeServiceHub.com.vaps.interfaces;
using FeeServiceHub.com.vaps.services;
using PreadmissionDTOs.com.vaps.Fees;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class FeeAdjustmentFacade : Controller
    {
        public FeeAdjustmentInterface _org;
        public FeeAdjustmentFacade(FeeAdjustmentInterface orga)
        {
            _org = orga;
        }

        [HttpPost]
        [Route("getalldetails")]
        public FeeStudentAdjustmentDTO Getdet([FromBody] FeeStudentAdjustmentDTO data)
        {
            return _org.getdata(data);
        }

        [HttpPost]
        [Route("getclass")]
        public FeeStudentAdjustmentDTO Getclass([FromBody]FeeStudentAdjustmentDTO data)
        {
            return _org.getdataclassdet(data);
        }
        [HttpPost]
        [Route("getsection")]
        public FeeStudentAdjustmentDTO Getsection([FromBody]FeeStudentAdjustmentDTO data)
        {
            return _org.getdatasectiondet(data);
        }

        [HttpPost]
        [Route("getstudent")]
        public FeeStudentAdjustmentDTO GetStudent([FromBody]FeeStudentAdjustmentDTO data)
        {
            return _org.getdatastudentdet(data);
        }

        [HttpPost]
        [Route("getbothgroup")]
        public FeeStudentAdjustmentDTO Getbothgroup([FromBody]FeeStudentAdjustmentDTO data)
        {
            return _org.getdatabothgroupdet(data);
        }

        [HttpPost]
        [Route("getfromhead")]
        public FeeStudentAdjustmentDTO Getfromhead([FromBody]FeeStudentAdjustmentDTO data)
        {
            return _org.getdatafromheaddet(data);
        }

        [HttpPost]
        [Route("gettohead")]
        public FeeStudentAdjustmentDTO Gettohead([FromBody]FeeStudentAdjustmentDTO data)
        {
            return _org.getdatatoheaddet(data);
        }

       
        [Route("savedata")]
        public FeeStudentAdjustmentDTO savedataa([FromBody] FeeStudentAdjustmentDTO data)
        {

            return _org.savedatadelegate(data);
        }
        [Route("getpagedetails/{id:int}")]
        public FeeStudentAdjustmentDTO getpagedetails(int id)
        {
            // id = 12;
            return _org.getpageedit(id);
        }
        //[HttpDelete]
        //[Route("deletedetails/{id:int}")]
        //public FeeStudentAdjustmentDTO Deleterec(int id)
        //{
        //    return _org.deleterec(id);
        //}
        //[HttpPost]
        //[Route("deactivate")]
        //public FeeStudentAdjustmentDTO Deactivate([FromBody] FeeStudentAdjustmentDTO id)
        //{
        //    // id = 12;
        //    return _org.deactivate(id);
        //}
        [HttpDelete]
        [Route("deletedetails/{id:int}")]
        public FeeStudentAdjustmentDTO Deleterec(int id)
        {
            return _org.deleterec(id);
        }
        [HttpPost]
        [Route("searching")]
        public FeeStudentAdjustmentDTO searching([FromBody] FeeStudentAdjustmentDTO data)
        {
            return _org.searching(data);
        }
    }
}
