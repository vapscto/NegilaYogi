using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.TT;

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class BifurcationController : Controller
    {
        BifurcationDelegate ad = new BifurcationDelegate();

        // GET: api/Academic/5
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public TT_Bifurcation_DTO Get([FromQuery] int id)
        {


            TT_Bifurcation_DTO dto = new TT_Bifurcation_DTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.getAcademicdata(dto);
        }

        [Route("getdetails/{id:int}")]
        public TT_Bifurcation_DTO getdetails(int id)
        {
            TT_Bifurcation_DTO dto = new TT_Bifurcation_DTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            dto.TTB_Id = id;
            return ad.getdetails(dto);
        }

        [Route("getalldetailsviewrecords/{id:int}")]
        public TT_Bifurcation_DTO getalldetailsviewrecords(int id)
        {
            TT_Bifurcation_DTO dto = new TT_Bifurcation_DTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.TTB_Id = id;
            return ad.getalldetailsviewrecords(dto);
        }

        // POST: api/Academic
        [HttpPost]
        [Route("savedetail/")]
        public TT_Bifurcation_DTO Post([FromBody]TT_Bifurcation_DTO ac)
        {
            ac.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));        
            return ad.savedetails(ac);

        }

        [Route("getClassdetails/")]
        public TT_Bifurcation_DTO getClassdetails([FromBody]TT_Bifurcation_DTO ac)
        {
            ac.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.getClassdetails(ac);
        }

        [Route("deletedetails/{id:int}")]
        public TT_Bifurcation_DTO deletedetails(int id)
        {
            TT_Bifurcation_DTO dto = new TT_Bifurcation_DTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.TTB_Id = id;
            return ad.deletedetails(dto);
        }


    }
}
