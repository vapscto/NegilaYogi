using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.TT.College;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.TT;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.TT.College
{
    [Route("api/[controller]")]
    public class ClgPeriodAllocationController : Controller
    {
        ClgPeriodAllocationDelegate objdelegate = new ClgPeriodAllocationDelegate();
        [Route("save_period")]
        public ClgPeriodAllocation_DTO save_period([FromBody] ClgPeriodAllocation_DTO periodpage)
        {
            // periodpage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            periodpage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.save_period(periodpage);
        }



        [HttpGet]
        [Route("getalldetails")]
        public ClgPeriodAllocation_DTO Get([FromQuery] int id)
        {
            ClgPeriodAllocation_DTO data = new ClgPeriodAllocation_DTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            // data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return objdelegate.getdetails(data);
        }

        //[Route("get_catg")]
        //public ClgPeriodAllocation_DTO getcategories([FromBody] ClgPeriodAllocation_DTO data)
        //{
        //    //data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
        //    data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    return objdelegate.getcategories(data);
        //}



        [Route("get_catg")]
        public ClgPeriodAllocation_DTO getcategories([FromBody] ClgPeriodAllocation_DTO data)
        {
            //data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getcategories(data);
        }
        [HttpPost]
        [Route("getperiod_class")]
        public ClgPeriodAllocation_DTO getperiod_class([FromBody] ClgPeriodAllocation_DTO data)
        {
            // data.ASAMY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getperiod_class(data);
        }




        [Route("savedetail")]
        public ClgPeriodAllocation_DTO savedetail([FromBody] ClgPeriodAllocation_DTO periodpage)
        {
            // periodpage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            periodpage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.savedetail(periodpage);
        }




        [Route("deactivate")]
        public ClgPeriodAllocation_DTO deactvate([FromBody] ClgPeriodAllocation_DTO id)
        {
            return objdelegate.deactivate(id);
        }
        [HttpPost]
        [Route("deactivate1")]
        public ClgPeriodAllocation_DTO deactvate1([FromBody] ClgPeriodAllocation_DTO id)
        {
            return objdelegate.deactivate1(id);
        }









    }
}
