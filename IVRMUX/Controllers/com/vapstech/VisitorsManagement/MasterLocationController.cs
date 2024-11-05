using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.VisitorsManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VisitorsManagement;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.VisitorsManagement
{
    [Route("api/[controller]")]
    public class MasterLocationController : Controller
    {
        // GET: api/<controller>

        public MasterLocationDelegate _objdel = new MasterLocationDelegate();

        [Route("getdetails/{id:int}")]
        public Visitor_Management_Master_Location_DTO getdetails(int id)
        {
            Visitor_Management_Master_Location_DTO data = new Visitor_Management_Master_Location_DTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return _objdel.getdetails(data);
        }

        [Route("saveRecorddata")]
        public Visitor_Management_Master_Location_DTO saveRecorddata([FromBody]Visitor_Management_Master_Location_DTO data)
        {
            
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.saveRecorddata(data);
        }

        [Route("editrecord")]
        public Visitor_Management_Master_Location_DTO editrecord([FromBody]Visitor_Management_Master_Location_DTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return _objdel.editrecord(data);
        }

        [Route("deactiveY")]
        public Visitor_Management_Master_Location_DTO deactiveY([FromBody]Visitor_Management_Master_Location_DTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return _objdel.deactiveY(data);
        }

        
    }
}
