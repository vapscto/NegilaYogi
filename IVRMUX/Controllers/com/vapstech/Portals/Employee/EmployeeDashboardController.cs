using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.Portals.Employee;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Portals.Employee
{
    [Route("api/[controller]")]
    public class EmployeeDashboardController : Controller
    {
        EmployeeDashboardDelegate objdelegate = new EmployeeDashboardDelegate();
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [Route("getalldetails")]
        public EmployeeDashboardDTO getalldetails(EmployeeDashboardDTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.HRME_Id = Convert.ToInt32(HttpContext.Session.GetInt32("HRME_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Year = Convert.ToString(HttpContext.Session.GetString("ASMAY_Year"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.PaymentNootificationStaff = Convert.ToInt64(HttpContext.Session.GetInt32("PaymentNootificationStaff"));
            return objdelegate.getalldetails(data);
        }

        [Route("saveakpkfile")]
        public EmployeeDashboardDTO saveakpkfile([FromBody]EmployeeDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.IVRMUL_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdelegate.saveakpkfile(data);
        }

        [Route("viewnotice")]
        public EmployeeDashboardDTO viewnotice([FromBody]EmployeeDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.IVRMUL_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdelegate.viewnotice(data);
        }

        [Route("onclick_notice")]
        public EmployeeDashboardDTO onclick_notice([FromBody]EmployeeDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.HRME_Id = Convert.ToInt32(HttpContext.Session.GetInt32("HRME_Id"));
            if (data.OnClickOrOnChange == "OnClick")
            {
                data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            }
            data.ASMAY_Year = Convert.ToString(HttpContext.Session.GetString("ASMAY_Year"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.PaymentNootificationStaff = Convert.ToInt64(HttpContext.Session.GetInt32("PaymentNootificationStaff"));
            return objdelegate.onclick_notice(data);
        }

        [Route("onclick_events")]
        public EmployeeDashboardDTO onclick_events([FromBody]EmployeeDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.HRME_Id = Convert.ToInt32(HttpContext.Session.GetInt32("HRME_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Year = Convert.ToString(HttpContext.Session.GetString("ASMAY_Year"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.PaymentNootificationStaff = Convert.ToInt64(HttpContext.Session.GetInt32("PaymentNootificationStaff"));
            return objdelegate.onclick_events(data);
        }

        [Route("onclick_asset")]
        public EmployeeDashboardDTO onclick_asset([FromBody]EmployeeDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.HRME_Id = Convert.ToInt32(HttpContext.Session.GetInt32("HRME_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Year = Convert.ToString(HttpContext.Session.GetString("ASMAY_Year"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.PaymentNootificationStaff = Convert.ToInt64(HttpContext.Session.GetInt32("PaymentNootificationStaff"));
            return objdelegate.onclick_asset(data);
        }

    }
}
