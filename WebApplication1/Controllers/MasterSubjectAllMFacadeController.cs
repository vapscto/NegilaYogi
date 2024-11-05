using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Interfaces;
using PreadmissionDTOs;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class MasterSubjectAllMFacadeController : Controller
    {
        // GET: api/values
        public MasterSubjectAllMInterface _mast;
        public MasterSubjectAllMFacadeController(MasterSubjectAllMInterface master)
        {
            _mast = master;
        }
        [HttpGet]
        [Route("getdetails")]
        
        public MasterSubjectAllMDTO getdetails([FromBody]MasterSubjectAllMDTO mas)
        {
            return _mast.GetMasterSubDetails(mas);
        }
      
        [Route("getalldetails/{id:int}")]

        public MasterSubjectAllMDTO getalldetails(int id)
        {
            return _mast.getalldetails(id);
        }
        [Route("Editdetails/{id:int}")]
        public MasterSubjectAllMDTO Getmasterdetails(int id)
        {
            return _mast.EditMasterSubDetails(id);
        }
        [HttpPost]
        [Route("savedetail")]
        public MasterSubjectAllMDTO Post([FromBody]MasterSubjectAllMDTO mast)
        {
            return _mast.SaveMasterSubDetails(mast);
        }
        [Route("validateordernumber")]
        public MasterSubjectAllMDTO validateordernumber([FromBody]MasterSubjectAllMDTO mast)
        {
            return _mast.validateordernumber(mast);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        [Route("Deletedetails/{id:int}")]
        public MasterSubjectAllMDTO Deletedetails(int id)
        {
            return _mast.DeleteMasterSubDetails(id);
        }

    }
}
