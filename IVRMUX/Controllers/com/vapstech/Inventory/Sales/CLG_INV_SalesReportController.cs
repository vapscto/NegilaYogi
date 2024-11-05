using IVRMUX.Delegates.com.vapstech.Inventory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Controllers.com.vapstech.Inventory
{
    [Route("api/[controller]")]
    public class CLG_INV_SalesReportController : Controller
    {
        CLG_INV_SalesReportDelegate _delegate = new CLG_INV_SalesReportDelegate();

        [HttpGet]
        [Route("getloaddata/{id:int}")]
        public INV_T_SalesDTO getloaddata(int id)
        {
            INV_T_SalesDTO data = new INV_T_SalesDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            return _delegate.getloaddata(data);
        }

        [Route("mainradiochange")]
        public INV_T_SalesDTO mainradiochange([FromBody] INV_T_SalesDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delegate.mainradiochange(data);
        }
        [Route("getbranchlist")]
        public INV_T_SalesDTO getbranchlist([FromBody] INV_T_SalesDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delegate.getbranchlist(data);
        }
        [Route("getsemesterlist")]
        public INV_T_SalesDTO getsemesterlist([FromBody] INV_T_SalesDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delegate.getsemesterlist(data);
        }
        [Route("getStudentlist")]
        public INV_T_SalesDTO getStudentlist([FromBody] INV_T_SalesDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delegate.getStudentlist(data);
        }
        [Route("onreport")]
        public INV_T_SalesDTO onreport([FromBody] INV_T_SalesDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.onreport(data);
        }



    }
}
