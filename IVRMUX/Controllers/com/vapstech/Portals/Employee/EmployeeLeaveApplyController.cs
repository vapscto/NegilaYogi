using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.Portals.Employee;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Portals.Employee
{
    [Route("api/[controller]")]
    public class EmployeeLeaveApplyController : Controller
    {
        EmployeeLeaveApplyDelegate lcd = new EmployeeLeaveApplyDelegate();

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

       
        // GET: api/values


        [Route("getonlineLeave/{id:int}")]
        public EmployeeDashboardDTO getonlineLeave(int id)
        {
            EmployeeDashboardDTO lv = new EmployeeDashboardDTO();
            lv.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            lv.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return lcd.getonlineLeave(lv);
        }

        [Route("save")]
        public EmployeeDashboardDTO onlineleavesave([FromBody] EmployeeDashboardDTO test)
        {
            test.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return lcd.savedetail(test);
        }

        //======================== TC Class teacher approval
        [Route("getdata_CTA/{id:int}")]
        public Adm_TC_Approval_DTO getdata_CTA (int id)
        {
            Adm_TC_Approval_DTO dto = new Adm_TC_Approval_DTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return lcd.getdata_CTA(dto);
        }

        [Route("SaveEdit_CTA")]
        public Adm_TC_Approval_DTO SaveEdit_CTA([FromBody] Adm_TC_Approval_DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return lcd.SaveEdit_CTA(dto);
        }

        [Route("details_CTA/{id:int}")]
        public Adm_TC_Approval_DTO details_CTA(int id)
        {
            Adm_TC_Approval_DTO dto = new Adm_TC_Approval_DTO();
            dto.ATCCTAPP_Id = id;
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return lcd.details_CTA(dto);
        }

        [Route("deactivate_CTA")]
        public Adm_TC_Approval_DTO deactivate_CTA([FromBody] Adm_TC_Approval_DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return lcd.deactivate_CTA(dto);
        }
         [Route("getstudetails_CTA")]
        public Adm_TC_Approval_DTO getstudetails_CTA([FromBody] Adm_TC_Approval_DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return lcd.getstudetails_CTA(dto);
        }

        //======================== TC Library approval
        [Route("getdata_LIB/{id:int}")]
        public Adm_TC_Approval_DTO getdata_LIB(int id)
        {
            Adm_TC_Approval_DTO dto = new Adm_TC_Approval_DTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return lcd.getdata_LIB(dto);
        }

        [Route("SaveEdit_LIB")]
        public Adm_TC_Approval_DTO SaveEdit_LIB([FromBody] Adm_TC_Approval_DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return lcd.SaveEdit_LIB(dto);
        }

        [Route("details_LIB/{id:int}")]
        public Adm_TC_Approval_DTO details_LIB(int id)
        {
            Adm_TC_Approval_DTO dto = new Adm_TC_Approval_DTO();
            dto.ATCLIBAPP_Id = id;
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return lcd.details_LIB(dto);
        }

        [Route("deactivate_LIB")]
        public Adm_TC_Approval_DTO deactivate_LIB([FromBody] Adm_TC_Approval_DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return lcd.deactivate_LIB(dto);
        }
         [Route("getstudetails_LIB")]
        public Adm_TC_Approval_DTO getstudetails_LIB([FromBody] Adm_TC_Approval_DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return lcd.getstudetails_LIB(dto);
        }


        //======================== TC FEE approval
        [Route("getdata_FEE/{id:int}")]
        public Adm_TC_Approval_DTO getdata_FEE(int id)
        {
            Adm_TC_Approval_DTO dto = new Adm_TC_Approval_DTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return lcd.getdata_FEE(dto);
        }

        [Route("SaveEdit_FEE")]
        public Adm_TC_Approval_DTO SaveEdit_FEE([FromBody] Adm_TC_Approval_DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return lcd.SaveEdit_FEE(dto);
        }

        [Route("detailsFEE/{id:int}")]
        public Adm_TC_Approval_DTO details_FEE(int id)
        {
            Adm_TC_Approval_DTO dto = new Adm_TC_Approval_DTO();
            dto.ATCFAPP_Id = id;
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return lcd.details_FEE(dto);
        }

        [Route("deactivate_FEE")]
        public Adm_TC_Approval_DTO deactivate_FEE([FromBody] Adm_TC_Approval_DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return lcd.deactivate_FEE(dto);
        }


        [Route("getstudetails_FEE")]
        public Adm_TC_Approval_DTO getstudetails_FEE([FromBody] Adm_TC_Approval_DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return lcd.getstudetails_FEE(dto);
        }

         [Route("feeheaddetails_FEE")]
        public Adm_TC_Approval_DTO feeheaddetails_FEE([FromBody] Adm_TC_Approval_DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return lcd.feeheaddetails_FEE(dto);
        }
          [Route("feenot_approval_FEE")]
        public Adm_TC_Approval_DTO feenot_approval_FEE([FromBody] Adm_TC_Approval_DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return lcd.feenot_approval_FEE(dto);
        }

        //======================== PDA Class teacher approval
        [Route("getdata_PDA/{id:int}")]
        public Adm_TC_Approval_DTO getdata_PDA(int id)
        {
            Adm_TC_Approval_DTO dto = new Adm_TC_Approval_DTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return lcd.getdata_PDA(dto);
        }

        [Route("SaveEdit_PDA")]
        public Adm_TC_Approval_DTO SaveEdit_PDA([FromBody] Adm_TC_Approval_DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return lcd.SaveEdit_PDA(dto);
        }

        [Route("details_PDA/{id:int}")]
        public Adm_TC_Approval_DTO details_PDA(int id)
        {
            Adm_TC_Approval_DTO dto = new Adm_TC_Approval_DTO();
            dto.ATCCTAPP_Id = id;
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return lcd.details_PDA(dto);
        }

        [Route("deactivate_PDA")]
        public Adm_TC_Approval_DTO deactivate_PDA([FromBody] Adm_TC_Approval_DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return lcd.deactivate_PDA(dto);
        }
        [Route("getstudetails_PDA")]
        public Adm_TC_Approval_DTO getstudetails_PDA([FromBody] Adm_TC_Approval_DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return lcd.getstudetails_PDA(dto);
        }

    }
}
