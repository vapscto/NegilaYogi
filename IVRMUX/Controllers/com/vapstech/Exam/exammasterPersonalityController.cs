
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
    public class exammasterPersonalityController : Controller
    {

        exammasterPersonalityDelegates exammasterPersonalitydelStr = new exammasterPersonalityDelegates();
      
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        // Master Personality
        [Route("Getdetails")]
        public exammasterpersonalityDTO Getdetails(exammasterpersonalityDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return exammasterPersonalitydelStr.Getdetails(data);            
        }

        [Route("editdetails/{id:int}")]
        public exammasterpersonalityDTO editdetails(int ID)
        {
            return exammasterPersonalitydelStr.editdetails(ID);
        }
        
        [Route("validateordernumber")]
        public exammasterpersonalityDTO validateordernumber([FromBody] exammasterpersonalityDTO data)
        {
          //  data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return exammasterPersonalitydelStr.validateordernumber(data);
        }

        [Route("savedetails")]
        public exammasterpersonalityDTO savedetails([FromBody] exammasterpersonalityDTO data)
        {     
            data.MI_Id= Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return exammasterPersonalitydelStr.savedetails(data);
        }

        [Route("deactivate")]
        public exammasterpersonalityDTO deactivate([FromBody] exammasterpersonalityDTO data)
        {
            return exammasterPersonalitydelStr.deactivate(data);         
        }

        // Student Personlity Mapping 

        [Route("studentdataload")]
        public exammasterpersonalityDTO studentdataload(exammasterpersonalityDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return exammasterPersonalitydelStr.studentdataload(data);
        }

        [Route("onchangeyear")]
        public exammasterpersonalityDTO onchangeyear([FromBody]exammasterpersonalityDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));            
            return exammasterPersonalitydelStr.onchangeyear(data);
        }

        [Route("onchangeclass")]
        public exammasterpersonalityDTO onchangeclass([FromBody]exammasterpersonalityDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return exammasterPersonalitydelStr.onchangeclass(data);
        }

        [Route("onchangesection")]
        public exammasterpersonalityDTO onchangesection([FromBody]exammasterpersonalityDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return exammasterPersonalitydelStr.onchangesection(data);
        }
        [Route("searchdata")]
        public exammasterpersonalityDTO searchdata([FromBody]exammasterpersonalityDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return exammasterPersonalitydelStr.searchdata(data);
        }
        [Route("savemapping")]
        public exammasterpersonalityDTO savemapping([FromBody]exammasterpersonalityDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return exammasterPersonalitydelStr.savemapping(data);
        }
        [Route("editmappingdetails")]
        public exammasterpersonalityDTO editmappingdetails([FromBody]exammasterpersonalityDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return exammasterPersonalitydelStr.editmappingdetails(data);
        }

        


    }

}
