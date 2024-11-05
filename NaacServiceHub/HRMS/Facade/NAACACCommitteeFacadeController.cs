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
    public class NAACACCommitteeFacadeController : Controller
    {
        public NAACACCommitteeInterface _ads;

        public NAACACCommitteeFacadeController(NAACACCommitteeInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public NAACACCommitteeDTO getinitialdata([FromBody]NAACACCommitteeDTO dto)
        {
            return _ads.getBasicData(dto);
        }

        // POST api/values
        [HttpPost]
        public NAACACCommitteeDTO Post([FromBody]NAACACCommitteeDTO dto)
        {
            return _ads.SaveUpdate(dto);
        }

        [Route("getRecordById/{id:int}")]

        public NAACACCommitteeDTO getcatgrydet(int id)
        {
            // id = 12;
            return _ads.editData(id);
        }
        [Route("deactivateRecordById")]
        public NAACACCommitteeDTO deactivateRecordById([FromBody]NAACACCommitteeDTO dto)
        {
            return _ads.deactivate(dto);
        }
    }
}
