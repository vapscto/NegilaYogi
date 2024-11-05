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
    public class JSHSAdmissionCertificateController : Controller
    {
        JSHSAdmissionCertificateDelegate del = new JSHSAdmissionCertificateDelegate();
        [HttpGet]
        [Route("getdata/{id:int}")]
        public JSHSAdmissionCertificate_DTO getdata(int id)
        {
            JSHSAdmissionCertificate_DTO data = new JSHSAdmissionCertificate_DTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getdata(data);
        }

        [Route("searchfilter")]
        public JSHSAdmissionCertificate_DTO searchfilter([FromBody] JSHSAdmissionCertificate_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.searchfilter(data);
        }
        [Route("onchangeyear")]
        public JSHSAdmissionCertificate_DTO onchangeyear([FromBody] JSHSAdmissionCertificate_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.onchangeyear(data);
        }

        [Route("onchangeclass")]
        public JSHSAdmissionCertificate_DTO onchangeclass([FromBody] JSHSAdmissionCertificate_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.onchangeclass(data);
        }

        [Route("onchangesection")]
        public JSHSAdmissionCertificate_DTO onchangesection([FromBody] JSHSAdmissionCertificate_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.onchangesection(data);
        }

        [Route("Studdetails")]

        public JSHSAdmissionCertificate_DTO getStudData([FromBody] JSHSAdmissionCertificate_DTO stuDTO)
        {
            stuDTO.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            stuDTO.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getStudData(stuDTO);
        }
    }
}