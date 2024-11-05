
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ExamServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.Exam;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]

    public class mastersubsubjectFacadeController : Controller
    {
        public mastersubsubjectInterface _mastersubsubject;
        public mastersubsubjectFacadeController(mastersubsubjectInterface mastersubsubject)
        {
            _mastersubsubject = mastersubsubject;
        }
    
        [Route("Getdetails")]
        public mastersubsubjectDTO Getdetails([FromBody]mastersubsubjectDTO data)//int IVRMM_Id
        {           
            return _mastersubsubject.Getdetails(data);           
        }

        [Route("savedetails")]
        public mastersubsubjectDTO savedetails([FromBody]mastersubsubjectDTO data)
        {           
            return _mastersubsubject.savedetails(data);
        }
        
        [Route("validateordernumber")]
        public mastersubsubjectDTO validateordernumber([FromBody]mastersubsubjectDTO data)
        {
            return _mastersubsubject.validateordernumber(data);
        }

        [Route("deactivate")]
        public mastersubsubjectDTO deactivate([FromBody] mastersubsubjectDTO data)
        {          
            return _mastersubsubject.deactivate(data);
        }
        
        [Route("editdeatils/{id:int}")]
        public mastersubsubjectDTO editdeatils(int ID)
        {           
            return _mastersubsubject.editdeatils(ID);
        }

     
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {

        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {

        }

    }
}
