using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.admission;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.admission
{
    [Route("api/[controller]")]
    public class ReligionCasteCategoryReportController : Controller
    {
        ReligionCasteCategoryReportDelegate del = new ReligionCasteCategoryReportDelegate();

        [Route("loaddata/{id:int}")]
        public ReligionCasteCategoryReport_DTO loaddata(int id)
        {
            ReligionCasteCategoryReport_DTO data = new ReligionCasteCategoryReport_DTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.loaddata(data);
        }

        [Route("showdetails")]
        public ReligionCasteCategoryReport_DTO showdetails([FromBody] ReligionCasteCategoryReport_DTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.showdetails(data);
        }
    }
}
