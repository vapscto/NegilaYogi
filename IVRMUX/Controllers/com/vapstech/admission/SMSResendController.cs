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
    public class SMSResendController : Controller
    {

        SMSResendDelegates mastercastedelStr = new SMSResendDelegates();

      
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        [Route("Getdetailsstatus/")]
        public SMSResendDTO Getdetailsstatus(SMSResendDTO SMSResendDTO)
        {
            SMSResendDTO.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            SMSResendDTO.IVRMUL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return mastercastedelStr.Getdetailsstatus(SMSResendDTO);            
        }
        

        [Route("Gettransnostatus/")]
        public SMSResendDTO Gettransnostatus([FromBody] SMSResendDTO SMSResendDTO)
        {
            SMSResendDTO.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            SMSResendDTO.IVRMUL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return mastercastedelStr.Gettransnostatus(SMSResendDTO);            
        }
        [Route("getstatusreport/")]
        public SMSResendDTO getstatusreport([FromBody] SMSResendDTO SMSResendDTO)
        {
            SMSResendDTO.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            SMSResendDTO.IVRMUL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return mastercastedelStr.getstatusreport(SMSResendDTO);            
        }




        [Route("Getdetails/")]
        public SMSResendDTO Getdetails(SMSResendDTO SMSResendDTO)
        {
            SMSResendDTO.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            SMSResendDTO.IVRMUL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return mastercastedelStr.Getdetails(SMSResendDTO);            
        }
        [Route("Gettransno/")]
        public SMSResendDTO Gettransno( [FromBody] SMSResendDTO SMSResendDTO)
        {
            SMSResendDTO.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            SMSResendDTO.IVRMUL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return mastercastedelStr.Gettransno(SMSResendDTO);
        }
        [Route("showdata/")]
        public SMSResendDTO showdata([FromBody] SMSResendDTO SMSResendDTO)
        {
            SMSResendDTO.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            SMSResendDTO.IVRMUL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return mastercastedelStr.showdata(SMSResendDTO);
        }

        [Route("resendMsg/")]
        public SMSResendDTO resendMsg([FromBody] SMSResendDTO SMSResendDTO)
        {
            SMSResendDTO.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            SMSResendDTO.IVRMUL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return mastercastedelStr.resendMsg(SMSResendDTO);
        }

        

        [Route("GetSelectedRowdetails/{id:int}")]
        public SMSResendDTO GetSelectedRowDetails(int ID)
        {
            return mastercastedelStr.GetSelectedRowDetails(ID);
        }


        [HttpPost]      
        public SMSResendDTO SMSResendDTO([FromBody] SMSResendDTO MMD)
        {
            MMD.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return mastercastedelStr.mastercasteData(MMD);            
        }

        [HttpDelete]
        [Route("MasterDeleteModulesDTO/{id:int}")]
        public SMSResendDTO SMSResendDTO(int ID)
        {
            return mastercastedelStr.MasterDeleteModulesData(ID);         
        }
    }

}
