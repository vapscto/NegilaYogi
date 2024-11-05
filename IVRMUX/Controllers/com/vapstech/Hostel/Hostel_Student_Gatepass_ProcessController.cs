using System;
using IVRMUX.Delegates.com.vapstech.Hostel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Hostel;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Hostel
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class Hostel_Student_Gatepass_ProcessController : Controller
    {
        Hostel_Student_Gatepass_ProcessDelegate del = new Hostel_Student_Gatepass_ProcessDelegate();
        // GET: api/values
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public Hostel_Student_GatePassDTO getalldetails(int id)
        {
            Hostel_Student_GatePassDTO dto = new Hostel_Student_GatePassDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.LogInUserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return del.onloadgetdetails(dto);
        }
        // POST api/values

        [Route("empdetails")]
        public Hostel_Student_GatePassDTO empdetails([FromBody] Hostel_Student_GatePassDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            //dto.LogInUserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return del.empdetails(dto);
        }

        [HttpPost]
        [Route("approvedrecord")]
        public Hostel_Student_GatePassDTO approvedrecord([FromBody] Hostel_Student_GatePassDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.LogInUserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return del.approvedrecord(dto);
        }

        ////------------------  Approval Report------------------------------
        [Route("onloaddata/{id:int}")]
        public Hostel_Student_GatePassDTO Onload(int n)
        {
            Hostel_Student_GatePassDTO dto = new Hostel_Student_GatePassDTO();
            dto.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.LogInUserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.Onload(dto);
        }
        [HttpPost]
        [Route("getapprovalreport")]
        public Hostel_Student_GatePassDTO getapprovalreport([FromBody]Hostel_Student_GatePassDTO dto)
        {
            dto.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.LogInUserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getapprovalreport(dto);
        }



        //GatePass Admin Apply
        [HttpGet]
        [Route("getGatePassAdminApplyOnload/{id:int}")]
        public Hostel_Student_GatePassDTO getGatePassAdminApplyOnload(int id)
        {
            Hostel_Student_GatePassDTO dto = new Hostel_Student_GatePassDTO();
            dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return del.getGatePassAdminApplyOnload(dto);
        }

        [Route("SaveUpdate")]
        public Hostel_Student_GatePassDTO SaveUpdate([FromBody] Hostel_Student_GatePassDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return del.SaveUpdate(data);
        }
        [Route("UpdateStatus")]
        public Hostel_Student_GatePassDTO UpdateStatus([FromBody] Hostel_Student_GatePassDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return del.UpdateStatus(data);
        }
        [Route("ActiveDeactiveRecord")]
        public Hostel_Student_GatePassDTO ActiveDeactiveRecord([FromBody] Hostel_Student_GatePassDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return del.deactivate(data);
        }
        [Route("Edit")]
        public Hostel_Student_GatePassDTO Edit([FromBody] Hostel_Student_GatePassDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return del.Edit(data);
        }



    }
}
