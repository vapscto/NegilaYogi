using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Library.Reports;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Library;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Library.Reports
{
    [Route("api/[controller]")]
    public class Lib_stu_punch_reportController : Controller
    {
        Lib_stu_punch_reportDelegate LSP= new Lib_stu_punch_reportDelegate();
        // GET: /<controller>/  
        [Route("Getdetails/{id:int}")]
        public Lib_stu_punch_reportDTO Getdetails(int id)
        {
            Lib_stu_punch_reportDTO data = new Lib_stu_punch_reportDTO();
            //data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            //data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            //data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return LSP.Getdetails(data);
        }


        [Route("get_classes")]
        public Lib_stu_punch_reportDTO get_classes([FromBody]Lib_stu_punch_reportDTO data)
        {
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return LSP.get_classes(data);
        }
        [Route("get_sections")]
        public Lib_stu_punch_reportDTO get_cls_sections([FromBody]Lib_stu_punch_reportDTO data)
        {
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return LSP.get_sections(data);
        }


        [Route("get_students_category_grade")]
        public Lib_stu_punch_reportDTO get_students([FromBody]Lib_stu_punch_reportDTO data)
        {
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return LSP.get_students_category_grade(data);
        }
        [Route("get_report")]
        public Lib_stu_punch_reportDTO get_report([FromBody]Lib_stu_punch_reportDTO data)
        {
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.Roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return LSP.get_report(data);
        }
        //for-college
        [Route("onloadpage/{id:int}")]
        public Lib_stu_punch_reportDTO onloadpage(int id)
        {
            Lib_stu_punch_reportDTO data = new Lib_stu_punch_reportDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return LSP.onloadpage(data);
        }

        //load course
        [Route("loadcourse")]
        public Lib_stu_punch_reportDTO loadcourse([FromBody]Lib_stu_punch_reportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return LSP.loadcourse(data);
        }
        //load branch
        [Route("loadbranch")]
        public Lib_stu_punch_reportDTO loadbranch([FromBody]Lib_stu_punch_reportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return LSP.loadbranch(data);
        }
        //load semester
        [Route("loadsemester")]
        public Lib_stu_punch_reportDTO loadsemester([FromBody]Lib_stu_punch_reportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return LSP.loadsemester(data);
        }
        //load section
        [Route("loaadsection")]
        public Lib_stu_punch_reportDTO loaadsection([FromBody]Lib_stu_punch_reportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return LSP.loaadsection(data);
        }
        //load students
        [Route("loadstudents")]
        public Lib_stu_punch_reportDTO loadstudents([FromBody]Lib_stu_punch_reportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return LSP.loadstudents(data);
        }
        //report-clg
        [Route("clgpunchreport")]
        public Lib_stu_punch_reportDTO clgpunchreport([FromBody]Lib_stu_punch_reportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return LSP.clgpunchreport(data);
        }
    }
}
