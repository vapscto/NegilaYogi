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
    public class School_M_ClassFacadeController : Controller
    {
        public School_M_ClassInterface _M_Class;
        public School_M_ClassFacadeController(School_M_ClassInterface M_ClassB)
        {
            _M_Class = M_ClassB;
        }
        //// GET: api/values
        [Route("getalldetails/")]
        public School_M_ClassDTO getalldetails([FromBody] School_M_ClassDTO enqo)
        {
            return _M_Class.AllDropdownList(enqo);
        }
        [Route("getdetails/{id:int}")]
        public School_M_ClassDTO getSchool_M_Classdet(int id)
        {
            // id = 12;
            return _M_Class.getdetails(id);
        }
        // POST api/values
        [HttpPost]
        public School_M_ClassDTO Post([FromBody] School_M_ClassDTO M_Class)
        {
            return _M_Class.saveSchool_M_Class(M_Class);
        }

        [Route("getdetailsById/{id:int}")]
        public School_M_ClassDTO getSchool_M_ClassdetById(int id)
        {
            // id = 12;
            return _M_Class.getdetails(id);
        }
        [Route("deletedetails")]
        public School_M_ClassDTO deletedetails([FromBody]School_M_ClassDTO data)
        {
            return _M_Class.deletedetails(data);
        }
        // DELETE api/values/5
        [HttpDelete]
        [Route("deletedetails")]
        public School_M_ClassDTO Deleterec([FromBody]School_M_ClassDTO data)
        {
            return _M_Class.deleterec(data);
        }

       

        [Route("searchByColumn")]
        public School_M_ClassDTO searchByColumn([FromBody] School_M_ClassDTO dto)
        {
            // id = 12;
            return _M_Class.searchByColumn(dto);
        }
    }
}
