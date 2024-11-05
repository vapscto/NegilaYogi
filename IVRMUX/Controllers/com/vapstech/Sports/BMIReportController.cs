using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Sports;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Sports;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Sports
{
    [Route("api/[controller]")]
    public class BMIReportController : Controller
    {
        BMIReportDelegate delobj = new BMIReportDelegate();

        [Route("report")]
        public BMICalculationDTO report([FromBody]BMICalculationDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return delobj.report(data);
        }

        [Route("getDetails/{id:int}")]
        public BMICalculationDTO getDetails(int id)
        {
            BMICalculationDTO dto = new BMICalculationDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return delobj.getDetails(dto);
        }

        [Route("get_class")]
        public BMICalculationDTO get_class([FromBody]BMICalculationDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            

            return delobj.get_class(data);
        }


        [Route("get_section")]
        public BMICalculationDTO get_section([FromBody]BMICalculationDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return delobj.get_section(data);
        }

        [Route("getStudents")]
        public BMICalculationDTO getStudents([FromBody]BMICalculationDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return delobj.getStudents(data);
        }

    }
}
