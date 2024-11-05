using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;
using corewebapi18072016.Delegates.com.vapstech.admission;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.admission
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class HHSBonafiedCertificateController : Controller
    {
        HHSStudyCertificateDelegate adsd = new HHSStudyCertificateDelegate();
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5

        [Route("getdata/{id:int}")]
        public HHSStudyCertificateDTO getinitialdata(int id)
        {
            HHSStudyCertificateDTO data = new HHSStudyCertificateDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return adsd.getdetails(data);
        }

        [Route("Studdetails")]
        public HHSStudyCertificateDTO getStudData([FromBody] HHSStudyCertificateDTO stuDTO)
        {
            stuDTO.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            stuDTO.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return adsd.GetStudDataById(stuDTO);
        }

        [Route("MigrationCertificateStuddetails")]
        public HHSStudyCertificateDTO MigrationCertificateStuddetails([FromBody] HHSStudyCertificateDTO stuDTO)
        {
            stuDTO.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            stuDTO.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return adsd.MigrationCertificateStuddetails(stuDTO);
        }

        [HttpPost]
        [Route("getS")]
        public HHSStudyCertificateDTO getstudentlist([FromBody]HHSStudyCertificateDTO student)
        {
            student.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            student.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return adsd.getstudlist(student);
        }
        [Route("onacademicyearchange")]
        public HHSStudyCertificateDTO onacademicyearchange([FromBody] HHSStudyCertificateDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return adsd.onacademicyearchange(data);
        }

        [Route("searchfilter")]
        public HHSStudyCertificateDTO searchfilter([FromBody] HHSStudyCertificateDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return adsd.searchfilter(data);

        }
        [Route("getstudentname")]
        public HHSStudyCertificateDTO getstudentname([FromBody] HHSStudyCertificateDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return adsd.getstudentname(data);
        }

        // CertificateGeneratedReport

        [Route("CertificateGeneratedReportLoad/{id:int}")]
        public HHSStudyCertificateDTO CertificateGeneratedReportLoad(int id)
        {
            HHSStudyCertificateDTO data = new HHSStudyCertificateDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return adsd.CertificateGeneratedReportLoad(data);
        }

        [Route("GetCertificateGeneratedReport")]
        public HHSStudyCertificateDTO GetCertificateGeneratedReport([FromBody] HHSStudyCertificateDTO data)
        {            
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return adsd.GetCertificateGeneratedReport(data);
        }
    }
}
