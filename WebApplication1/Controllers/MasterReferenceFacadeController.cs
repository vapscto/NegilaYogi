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
    public class MasterReferenceFacadeController : Controller
    {
        public MasterReferenceInterface _mast;
        public MasterReferenceFacadeController(MasterReferenceInterface master)
        {
            _mast = master;
        }
        // GET: api/values
        [HttpGet]
        public MasterRefernceDTO Get(MasterRefernceDTO mas)
        {
            return _mast.GetMasterReferncDetails(mas);
        }
        [Route("Editdetails/{id:int}")]
        public MasterRefernceDTO Getmasterdetails(int id)
        {
            return _mast.EditMasterRefDetails(id);
        }


        

        // POST api/values
        [HttpPost]
        public MasterRefernceDTO Post([FromBody]MasterRefernceDTO mast)
        {
            return _mast.SaveMasterRefernceDetails(mast);
        }

        [HttpPost("{id}")]
        public MasterRefernceDTO Put(int id, [FromBody]MasterRefernceDTO value)
        {
            return _mast.getsearchdata(id, value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpGet("{id}")]
        [Route("Deletedetails/{id:int}")]
        public MasterRefernceDTO Deletedetails(int id)
        {
            return _mast.DeleteMasterReferncDetails(id);
        }
    }
}
