
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
    public class exammasterCoCurricularController : Controller
    {

        exammasterCoCurricularDelegates exammasterCoCurricularDelegatesStr = new exammasterCoCurricularDelegates();
      
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [Route("Getdetails")]
        public exammasterCoCurricularDTO Getdetails(exammasterCoCurricularDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return exammasterCoCurricularDelegatesStr.Getdetails(data);            
        }

        [Route("editdetails/{id:int}")]
        public exammasterCoCurricularDTO editdetails(int ID)
        {
            return exammasterCoCurricularDelegatesStr.editdetails(ID);
        }
        
        [Route("validateordernumber")]
        public exammasterCoCurricularDTO validateordernumber([FromBody] exammasterCoCurricularDTO data)
        {
          //  data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return exammasterCoCurricularDelegatesStr.validateordernumber(data);
        }

        [Route("savedetails")]
        public exammasterCoCurricularDTO savedetails([FromBody] exammasterCoCurricularDTO data)
        {     
            data.MI_Id= Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return exammasterCoCurricularDelegatesStr.savedetails(data);
        }

        [Route("deactivate")]
        public exammasterCoCurricularDTO deactivate([FromBody] exammasterCoCurricularDTO data)
        {
            return exammasterCoCurricularDelegatesStr.deactivate(data);         
        }


        // Student Cocurricular Mapping 

        [Route("studentdataload")]
        public exammasterCoCurricularDTO studentdataload(exammasterCoCurricularDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));            
            return exammasterCoCurricularDelegatesStr.studentdataload(data);
        }

        [Route("onchangeyear")]
        public exammasterCoCurricularDTO onchangeyear([FromBody]exammasterCoCurricularDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return exammasterCoCurricularDelegatesStr.onchangeyear(data);
        }

        [Route("onchangeclass")]
        public exammasterCoCurricularDTO onchangeclass([FromBody]exammasterCoCurricularDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return exammasterCoCurricularDelegatesStr.onchangeclass(data);
        }

        [Route("onchangesection")]
        public exammasterCoCurricularDTO onchangesection([FromBody]exammasterCoCurricularDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return exammasterCoCurricularDelegatesStr.onchangesection(data);
        }
        [Route("searchdata")]
        public exammasterCoCurricularDTO searchdata([FromBody]exammasterCoCurricularDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return exammasterCoCurricularDelegatesStr.searchdata(data);
        }
        [Route("savemapping")]
        public exammasterCoCurricularDTO savemapping([FromBody]exammasterCoCurricularDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return exammasterCoCurricularDelegatesStr.savemapping(data);
        }
        [Route("editmappingdetails")]
        public exammasterCoCurricularDTO editmappingdetails([FromBody]exammasterCoCurricularDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return exammasterCoCurricularDelegatesStr.editmappingdetails(data);
        }


    }

}
