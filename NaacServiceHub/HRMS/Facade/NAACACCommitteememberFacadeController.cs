using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;
using NaacServiceHub.HRMS.Interface;
using PreadmissionDTOs.HRMS;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.HRMS.Facade
{
    [Route("api/[controller]")]
    public class NAACACCommitteememberFacadeController : Controller
    {
        public NAACACCommitteememberInterface _ads;

        public NAACACCommitteememberFacadeController(NAACACCommitteememberInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public NAACACCommitteeMembersDTO getinitialdata([FromBody]NAACACCommitteeMembersDTO dto)
        {
            return _ads.getBasicData(dto);
        }

        // POST api/values
        [HttpPost]
        public NAACACCommitteeMembersDTO Post([FromBody]NAACACCommitteeMembersDTO dto)
        {
            return _ads.SaveUpdate(dto);
        }

        [Route("getRecordById/{id:int}")]

        public NAACACCommitteeMembersDTO getcatgrydet(int id)
        {
            // id = 12;
            return _ads.editData(id);
        }
        [Route("deactivateRecordById")]
        public NAACACCommitteeMembersDTO deactivateRecordById([FromBody]NAACACCommitteeMembersDTO dto)
        {
            return _ads.deactivate(dto);
        }
    }
}
