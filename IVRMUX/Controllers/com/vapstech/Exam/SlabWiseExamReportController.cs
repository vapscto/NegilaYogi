using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Exam;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Exam
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class SlabWiseExamReportController : Controller
    {
        SlabWiseExamReportDelegate slab = new SlabWiseExamReportDelegate();

        [Route("getsubjects")]
        public SlabWiseExamReportDTO getsubjects([FromBody] SlabWiseExamReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return slab.getsubjects(data);
        }
        [Route("getslabreport")]
        public SlabWiseExamReportDTO getslabreport([FromBody] SlabWiseExamReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return slab.getslabreport(data);
        }
    }
}
