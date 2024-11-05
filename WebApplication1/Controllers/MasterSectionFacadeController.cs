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
    public class MasterSectionFacadeController : Controller
    {
        public MasterSectionInterface _mast;
        public MasterSectionFacadeController(MasterSectionInterface master)
        {
            _mast = master;
        }
       [Route("getdata/{id:int}")]
       public MasterSectionDTO Get(int id)
        {
            return _mast.GetMasterscetionDetails(id);
        }

        // GET api/values/5

        [Route("Editdetails/{id:int}")]
        public  MasterSectionDTO Getmasterdetails(int id)
        {
            return _mast.EditMasterscetionDetails(id);
        }

        // POST api/values
        [HttpPost]
        public MasterSectionDTO Post([FromBody]MasterSectionDTO mast)
        {
            return _mast.SaveMasterscetionDetails(mast);
        }

        [HttpPost("{id}")]
        public MasterSectionDTO Put(int id, [FromBody]MasterSectionDTO value)
        {
            return _mast.getsearchdata(id, value);
        }

      

        // DELETE api/values/5
       
        [Route("Deletedetails")]
        public MasterSectionDTO Deletedetails([FromBody] MasterSectionDTO dto)
        {
            return _mast.DeleteMasterscetionDetails(dto);
        }
    }
}
