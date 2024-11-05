
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
    [Route("api/[controller]")]
    public class exammasterpointController : Controller
    {

        exammasterpointDelegates exammasterpointDelegatesStr = new exammasterpointDelegates();
      
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [Route("Getdetails")]
        public exammasterpointDTO Getdetails(exammasterpointDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return exammasterpointDelegatesStr.Getdetails(data);            
        }

        [Route("editdetails/{id:int}")]
        public exammasterpointDTO editdetails(int ID)
        {
            return exammasterpointDelegatesStr.editdetails(ID);
        }
        
        [Route("validateordernumber")]
        public exammasterpointDTO validateordernumber([FromBody] exammasterpointDTO data)
        {
          //  data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return exammasterpointDelegatesStr.validateordernumber(data);
        }

        [Route("savedetails")]
        public exammasterpointDTO savedetails([FromBody] exammasterpointDTO data)
        {     
            data.MI_Id= Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return exammasterpointDelegatesStr.savedetails(data);
        }

        [Route("deactivate")]
        public exammasterpointDTO deactivate([FromBody] exammasterpointDTO data)
        {
            return exammasterpointDelegatesStr.deactivate(data);         
        }

       
    }

}
