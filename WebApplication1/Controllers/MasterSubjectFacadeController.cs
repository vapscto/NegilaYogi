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
    public class MasterSubjectFacadeController : Controller
    {
        // GET: api/values
        public MasterSubjectInterface _mast;
        public MasterSubjectFacadeController(MasterSubjectInterface master)
        {
            _mast = master;
        }
        [Route("getdetails")]
        
        public MasterSubjectDTO getdetails([FromBody]MasterSubjectDTO mas)
        {
            return _mast.GetMasterSubDetails(mas);
        }
        [Route("Editdetails/{id:int}")]
        public MasterSubjectDTO Getmasterdetails(int id)
        {
            return _mast.EditMasterSubDetails(id);
        }
        [HttpPost]
        public MasterSubjectDTO Post([FromBody]MasterSubjectDTO mast)
        {
            return _mast.SaveMasterSubDetails(mast);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        [Route("Deletedetails/{id:int}")]
        public MasterSubjectDTO Deletedetails(int id)
        {
            return _mast.DeleteMasterSubDetails(id);
        }

    }
}
