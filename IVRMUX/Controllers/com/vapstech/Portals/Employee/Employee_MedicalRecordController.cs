using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Portals.Employee;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.Employee;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Portals.Employee
{
    [Route("api/[controller]")]
    public class Employee_MedicalRecord : Controller
    {
        // GET: api/values
        Employee_MedicalRecordDelegate _msg = new Employee_MedicalRecordDelegate();

        [Route("Getdetails/{id:int}")]
        public Employee_MedicalRecordDTO Getdetails(int id)
        {
            Employee_MedicalRecordDTO obj = new Employee_MedicalRecordDTO();
            obj.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            obj.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            obj.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            obj.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            obj.IVRMRT_Id = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            obj.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            return _msg.Getdetails(obj);
        }

        [Route("savedetail")]
        public Employee_MedicalRecordDTO savedetail([FromBody]Employee_MedicalRecordDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _msg.savedetail(data);
        }

        [Route("deactivate")]
        public Employee_MedicalRecordDTO deactivate([FromBody] Employee_MedicalRecordDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _msg.deactivate(data);
        }

        [Route("viewData")]
        public Employee_MedicalRecordDTO viewData([FromBody]Employee_MedicalRecordDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));          
            return _msg.viewData(data);
        }
        [Route("onclick_employee")]
        public Employee_MedicalRecordDTO onclick_employee([FromBody]Employee_MedicalRecordDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _msg.onclick_employee(data);
        }

    }
}
