using System;
using IVRMUX.Delegates.com.vapstech.Fees;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Fees;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Fees
{
    [Route("api/[controller]")]
    public class StudentFeeEnablePartialPaymentController : Controller
    {
        public StudentFeeEnablePartialPaymentDelegate objDel = new StudentFeeEnablePartialPaymentDelegate();

        [HttpGet]
        [Route("GetYearList/{id:int}")]
        public StudentFeeEnablePartialPaymentDTO GetYearList(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objDel.GetYearList(id);
        }
        [HttpPost]
        [Route("getsection")]   
        public StudentFeeEnablePartialPaymentDTO getsection([FromBody]StudentFeeEnablePartialPaymentDTO Data)
        {
            Data.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objDel.getsection(Data);
        }
        [Route("get_student")]
        public StudentFeeEnablePartialPaymentDTO get_student([FromBody]StudentFeeEnablePartialPaymentDTO Data)
        {
            Data.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objDel.get_student(Data);
        }
        [Route("savedata")]
        public StudentFeeEnablePartialPaymentDTO savedata([FromBody] StudentFeeEnablePartialPaymentDTO data)
        {
            data.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objDel.savedata(data);
        }
        [Route("deactivate")]
        public StudentFeeEnablePartialPaymentDTO deactivate([FromBody] StudentFeeEnablePartialPaymentDTO data)
        {
            data.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objDel.deactivate(data);
        }
    }
}
