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
    public class GeneralSiblingEmployeeMappingController : Controller
    {
        GeneralSiblingEmployeeMappingDelegate _delg = new GeneralSiblingEmployeeMappingDelegate();
           
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("getalldetails")]
        public GeneralSiblingEmployeeMappingDTO getInitialData([FromBody] GeneralSiblingEmployeeMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_ID = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.getalldetails(data);
        }

        [Route("selectradio")]
        public GeneralSiblingEmployeeMappingDTO getacademicyrdata([FromBody] GeneralSiblingEmployeeMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_ID = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.selectradio(data);
        }

        [Route("onstudentnamechange")]
        public GeneralSiblingEmployeeMappingDTO onstudentnamechange([FromBody] GeneralSiblingEmployeeMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_ID = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.onstudentnamechange(data);
        }

        [Route("viewsiblingdetails")]
        public GeneralSiblingEmployeeMappingDTO viewsiblingdetails([FromBody] GeneralSiblingEmployeeMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_ID = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.viewsiblingdetails(data);
        }

        [Route("Deletedetails")]
        public GeneralSiblingEmployeeMappingDTO Delete([FromBody] GeneralSiblingEmployeeMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_ID = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.deleterec(data);
        }

        [Route("onselectstaff")]
        public GeneralSiblingEmployeeMappingDTO onselectstaff([FromBody] GeneralSiblingEmployeeMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_ID = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.onselectstaff(data);
        }

        [Route("DeletRecordemployee")]
        public GeneralSiblingEmployeeMappingDTO DeletRecordemployee([FromBody] GeneralSiblingEmployeeMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_ID = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.DeletRecordemployee(data);
        }

        [Route("viewsiblingdetailsemployee")]
        public GeneralSiblingEmployeeMappingDTO viewsiblingdetailsemployee([FromBody] GeneralSiblingEmployeeMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_ID = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.viewsiblingdetailsemployee(data);
        }

        [Route("savedata")]
        public GeneralSiblingEmployeeMappingDTO savedta([FromBody] GeneralSiblingEmployeeMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_ID = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.savedata(data);
        }       
           
    }
}
