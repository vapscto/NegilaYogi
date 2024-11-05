using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.NAAC.Reports;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.NAAC.Reports;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.NAAC.Reports
{
    [Route("api/[controller]")]
    public class NAACCriteria3ReportController : Controller
    {

        public NAACCriteria3ReportDelegate objdel = new NAACCriteria3ReportDelegate();

        [Route("getdata/{id:int}")]
        public NAACCriteria3ReportDTO getdata(int id)
        {
            NAACCriteria3ReportDTO data = new NAACCriteria3ReportDTO();
            data.MI_Id=Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));

            return objdel.getdata(data);
        }


        [Route("get_report")]
        public NAACCriteria3ReportDTO get_report([FromBody]NAACCriteria3ReportDTO data)
        {
            
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            return objdel.get_report(data);
        }
        
        [Route("get_report364")]
        public NAACCriteria3ReportDTO get_report364([FromBody]NAACCriteria3ReportDTO data)
        {
            
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            return objdel.get_report364(data);
        }
        [Route("reportIPR")]
        public NAACCriteria3ReportDTO reportIPR([FromBody]NAACCriteria3ReportDTO data)
        {
            
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            return objdel.reportIPR(data);
        }


     
        
    }
}
