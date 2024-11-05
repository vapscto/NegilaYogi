
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using DomainModel.Model.com.vaps.Exam;
using PreadmissionDTOs.com.vaps.Exam;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class mastersubsubjectController : Controller
    {

        mastersubsubjectDelegates mastersubsubjectdelStr = new mastersubsubjectDelegates();
      
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [Route("Getdetails")]
        public mastersubsubjectDTO Getdetails(mastersubsubjectDTO mastersubsubjectDTO)
        {
            mastersubsubjectDTO.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return mastersubsubjectdelStr.GetmastersubsubjectData(mastersubsubjectDTO);            
        }

        [Route("savedetails")]
        public mastersubsubjectDTO savedetails([FromBody] mastersubsubjectDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return mastersubsubjectdelStr.savedetails(data);
        }
        

        [Route("validateordernumber")]
        public mastersubsubjectDTO validateordernumber([FromBody] mastersubsubjectDTO data)
        {
            //data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return mastersubsubjectdelStr.validateordernumber(data);
        }


        [Route("deactivate")]
        public mastersubsubjectDTO deactivate([FromBody] mastersubsubjectDTO data)
        {
            
            return mastersubsubjectdelStr.deactivate(data);
        }

        
        [Route("editdeatils/{id:int}")]
        public mastersubsubjectDTO editdeatils(int ID)
        {
            return mastersubsubjectdelStr.editdeatils(ID);         
        }
    }

}
