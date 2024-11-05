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
    public class FeeWaivedOffFacade : Controller
    {
        public FeeWaivedOffInterface _org;
        public FeeWaivedOffFacade(FeeWaivedOffInterface orga)
        {
            _org = orga;
        }

        [HttpPost]
        [Route("getalldetails")]
        public FeeStudentWaiveOffDTO Getdet([FromBody] FeeStudentWaiveOffDTO data)
        {
            return _org.getdata(data);
        }

        [HttpPost]
        [Route("getstudent")]
        public FeeStudentWaiveOffDTO GetStudent([FromBody]FeeStudentWaiveOffDTO data)
        {
            return _org.getdatastudentdet(data);
        }

        [HttpPost]
        [Route("getgroup")]
        public FeeStudentWaiveOffDTO Getgroup([FromBody]FeeStudentWaiveOffDTO data)
        {
            return _org.getdatagroupdet(data);
        }

        [HttpPost]
        [Route("gethead")]
        public FeeStudentWaiveOffDTO Gethead([FromBody]FeeStudentWaiveOffDTO data)
        {
            return _org.getdataheaddet(data);
        }
        [Route("savedata")]
        public FeeStudentWaiveOffDTO savedataa([FromBody] FeeStudentWaiveOffDTO data)
        {

            return _org.savedatadelegate(data);
        }
        [Route("getpagedetails/{id:int}")]
        public FeeStudentWaiveOffDTO getpagedetails(int id)
        {
            // id = 12;
            return _org.getpageedit(id);
        }
      
        [HttpDelete]
        [Route("deletedetails/{id:int}")]
        public FeeStudentWaiveOffDTO Deleterec(int id)
        {
            return _org.deleterec(id);
        }
        [HttpPost]
        [Route("searching")]
        public FeeStudentWaiveOffDTO searching([FromBody] FeeStudentWaiveOffDTO data)
        {
            return _org.searching(data);
        }
    }
}
