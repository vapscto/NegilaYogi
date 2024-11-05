
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
using corewebapi18072016.Delegates.com.vapstech.Exam;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [Route("api/[controller]")]
    public class CoScholasticActivityAreasController : Controller
    {

        CoScholasticActivityAreasDelegates CoSActArdelStr = new CoScholasticActivityAreasDelegates();
      
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [Route("Getdetails")]
        public CoScholasticActivityAreasDTO Getdetails(CoScholasticActivityAreasDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return CoSActArdelStr.Getdetails(data);            
        }

        [Route("editdetails/{id:int}")]
        public CoScholasticActivityAreasDTO editdetails(int ID)
        {
            return CoSActArdelStr.editdetails(ID);
        }

        [Route("validateordernumber")]
        public CoScholasticActivityAreasDTO validateordernumber([FromBody] CoScholasticActivityAreasDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return CoSActArdelStr.validateordernumber(data);
        }

        [Route("savedetails")]
        public CoScholasticActivityAreasDTO savedetails([FromBody] CoScholasticActivityAreasDTO data)
        {     
            data.MI_Id= Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return CoSActArdelStr.savedetails(data);
        }

        [Route("deactivate")]
        public CoScholasticActivityAreasDTO deactivate([FromBody] CoScholasticActivityAreasDTO data)
        {
            return CoSActArdelStr.deactivate(data);         
        }

       
    }

}
