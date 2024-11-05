
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
    public class exammasterRemakController : Controller
    {

        exammasterRemarkDelegates exammasterRemarkDelegatesStr = new exammasterRemarkDelegates();
      
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [Route("Getdetails")]
        public exammasterRemarkDTO Getdetails(exammasterRemarkDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return exammasterRemarkDelegatesStr.Getdetails(data);            
        }

        [Route("editdetails/{id:int}")]
        public exammasterRemarkDTO editdetails(int ID)
        {            
            return exammasterRemarkDelegatesStr.editdetails(ID);
        }
        
        [Route("validateordernumber")]
        public exammasterRemarkDTO validateordernumber([FromBody] exammasterRemarkDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            //  data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return exammasterRemarkDelegatesStr.validateordernumber(data);
        }

        [Route("savedetails")]
        public exammasterRemarkDTO savedetails([FromBody] exammasterRemarkDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.MI_Id= Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return exammasterRemarkDelegatesStr.savedetails(data);
        }

        [Route("deactivate")]
        public exammasterRemarkDTO deactivate([FromBody] exammasterRemarkDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return exammasterRemarkDelegatesStr.deactivate(data);         
        }

        // Exam wise Student Remarks
        [Route("studentdataload")]
        public exammasterRemarkDTO studentdataload(exammasterRemarkDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return exammasterRemarkDelegatesStr.studentdataload(data);
        }
        [Route("onchangeyear")]
        public exammasterRemarkDTO onchangeyear([FromBody]exammasterRemarkDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return exammasterRemarkDelegatesStr.onchangeyear(data);
        }

        [Route("onchangeclass")]
        public exammasterRemarkDTO onchangeclass([FromBody]exammasterRemarkDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return exammasterRemarkDelegatesStr.onchangeclass(data);
        }

        [Route("onchangesection")]
        public exammasterRemarkDTO onchangesection([FromBody]exammasterRemarkDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return exammasterRemarkDelegatesStr.onchangesection(data);
        }
        [Route("searchdata")]
        public exammasterRemarkDTO searchdata([FromBody]exammasterRemarkDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return exammasterRemarkDelegatesStr.searchdata(data);
        }
        [Route("savemapping")]
        public exammasterRemarkDTO savemapping([FromBody]exammasterRemarkDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return exammasterRemarkDelegatesStr.savemapping(data);
        }
        [Route("editmappingdetails")]
        public exammasterRemarkDTO editmappingdetails([FromBody]exammasterRemarkDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return exammasterRemarkDelegatesStr.editmappingdetails(data);
        }

        [Route("ViewSubjectWiseRemarks")]
        public exammasterRemarkDTO ViewSubjectWiseRemarks([FromBody]exammasterRemarkDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return exammasterRemarkDelegatesStr.ViewSubjectWiseRemarks(data);
        }

        //Subject Wise Remarks

        [Route("Subjectwise_studentdataload")]
        public exammasterRemarkDTO Subjectwise_studentdataload(exammasterRemarkDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return exammasterRemarkDelegatesStr.Subjectwise_studentdataload(data);
        }

        [Route("Subjectwise_onchangeyear")]
        public exammasterRemarkDTO Subjectwise_onchangeyear([FromBody]exammasterRemarkDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return exammasterRemarkDelegatesStr.Subjectwise_onchangeyear(data);
        }

        [Route("Subjectwise_onchangeclass")]
        public exammasterRemarkDTO Subjectwise_onchangeclass([FromBody]exammasterRemarkDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return exammasterRemarkDelegatesStr.Subjectwise_onchangeclass(data);
        }

        [Route("Subjectwise_onchangesection")]
        public exammasterRemarkDTO Subjectwise_onchangesection([FromBody]exammasterRemarkDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return exammasterRemarkDelegatesStr.Subjectwise_onchangesection(data);
        }

        [Route("Subjectwise_onchangeexam")]
        public exammasterRemarkDTO Subjectwise_onchangeexam([FromBody]exammasterRemarkDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return exammasterRemarkDelegatesStr.Subjectwise_onchangeexam(data);
        }

        [Route("Subjectwise_searchdata")]
        public exammasterRemarkDTO Subjectwise_searchdata([FromBody]exammasterRemarkDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return exammasterRemarkDelegatesStr.Subjectwise_searchdata(data);
        }

        [Route("SubjectWise_savemapping")]
        public exammasterRemarkDTO SubjectWise_savemapping([FromBody]exammasterRemarkDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return exammasterRemarkDelegatesStr.SubjectWise_savemapping(data);
        }

        [Route("SubjectWise_editmappingdetails")]
        public exammasterRemarkDTO SubjectWise_editmappingdetails([FromBody]exammasterRemarkDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return exammasterRemarkDelegatesStr.SubjectWise_editmappingdetails(data);
        }        
    }
}
