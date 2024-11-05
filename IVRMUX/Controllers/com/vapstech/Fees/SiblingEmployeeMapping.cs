using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.Fees;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Fees;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Fees
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class SiblingEmployeeMapping : Controller
    {
        SiblingEmployeeMappingDelegate od = new SiblingEmployeeMappingDelegate();
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("getalldetails")]
        public Adm_M_Sibling getInitialData([FromBody] Adm_M_Sibling data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_ID = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return od.formload(data);
        }

        [Route("selectradio")]
        public Adm_M_Sibling getacademicyrdata([FromBody] Adm_M_Sibling data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_ID = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return od.selectacade(data);
        }
        [Route("onstudentnamechange")]
        public Adm_M_Sibling onstudentnamechange([FromBody] Adm_M_Sibling data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_ID = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return od.onstudentnamechange(data);
        }
        [Route("onselectstaff")]
        public Adm_M_Sibling onselectstaff([FromBody] Adm_M_Sibling data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_ID = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return od.onselectstaff(data);
        }

        [Route("onstudentnamechangerte")]
        public Adm_M_Sibling onstudentnamechangerte([FromBody] Adm_M_Sibling data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_ID = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return od.onstudentnamechangerte(data);
        }

        [Route("savedata")]
        public Adm_M_Sibling savedta([FromBody] Adm_M_Sibling data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_ID = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return od.savedt(data);
        }

        [Route("Deletedetails")]
        public Adm_M_Sibling Delete([FromBody] Adm_M_Sibling data)
        {            
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_ID = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return od.deleterec(data);
        }
        [Route("DeletRecordemployee")]
        public Adm_M_Sibling DeletRecordemployee([FromBody] Adm_M_Sibling data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_ID = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return od.DeletRecordemployee(data);
        }       
        
        [Route("viewsiblingdetails")]
        public Adm_M_Sibling viewsiblingdetails([FromBody] Adm_M_Sibling data)
        {           
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_ID = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return od.viewsiblingdetails(data);
        }
        [Route("viewsiblingdetailsemployee")]
        public Adm_M_Sibling viewsiblingdetailsemployee([FromBody] Adm_M_Sibling data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_ID = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return od.viewsiblingdetailsemployee(data);
        }
        [Route("checkfeegroup")]
        public Adm_M_Sibling checkfeegroup([FromBody] Adm_M_Sibling data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_ID = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return od.checkfeegroup(data);
        }
    }
}
