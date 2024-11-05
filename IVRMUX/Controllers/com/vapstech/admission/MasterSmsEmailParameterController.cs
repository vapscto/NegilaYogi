using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using DomainModel.Model.com.vaps.admission;
using PreadmissionDTOs.com.vaps.admission;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class MasterSmsEmailParameterController : Controller
    {

        MasterSmsEmailParameterDelegates castecategorydelStr = new MasterSmsEmailParameterDelegates();

      
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [Route("Getdetails/")]
        public MasterSmsEmailParameterDTO Getdetails(MasterSmsEmailParameterDTO MasterSmsEmailParameterDTO)

        {           

            return castecategorydelStr.GetcastecategoryData(MasterSmsEmailParameterDTO);            
        }

        [Route("edit")]
        public MasterSmsEmailParameterDTO edit([FromBody] MasterSmsEmailParameterDTO MMD)
        {
            return castecategorydelStr.edit(MMD);
        }


        [HttpPost]
        [Route("Savedata")]
        public MasterSmsEmailParameterDTO Savedata([FromBody] MasterSmsEmailParameterDTO MMD)
        {
            return castecategorydelStr.Savedata(MMD);         
        }

        [HttpDelete]
        [Route("deletedata/{id:int}")]
        public MasterSmsEmailParameterDTO deletedata(int ID)
        {
            return castecategorydelStr.deletedata(ID);         
        }


        ////HTML TEMPLATE////
        [Route("htmlGetdetails/")]
        public MasterSmsEmailParameterDTO htmlGetdetails(MasterSmsEmailParameterDTO data)

        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return castecategorydelStr.htmlGetcastecategoryData(data);
        }

        [Route("htmledit")]
        public MasterSmsEmailParameterDTO htmledit([FromBody] MasterSmsEmailParameterDTO MMD)
        {
        
            return castecategorydelStr.htmledit(MMD);
        }


        [HttpPost]
        [Route("htmlSavedata")]
        public MasterSmsEmailParameterDTO htmlSavedata([FromBody] MasterSmsEmailParameterDTO MMD)
        {
            MMD.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            MMD.User_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return castecategorydelStr.htmlSavedata(MMD);
        }

        [HttpPost]
        [Route("htmldeletedata")]
        public MasterSmsEmailParameterDTO htmldeletedata([FromBody] MasterSmsEmailParameterDTO MMD)
        {
            return castecategorydelStr.htmldeletedata(MMD);
        }
        

    }

}
