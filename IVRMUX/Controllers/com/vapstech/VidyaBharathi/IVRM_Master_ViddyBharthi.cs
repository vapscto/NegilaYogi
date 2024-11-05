using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.VidyaBharathi;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Scholorship;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.VidyaBharathi
{
    [Route("api/[controller]")]
    public class IVRM_Master_ViddyBharthi : Controller
    {

        IVRM_Master_ViddyBharthiDelegate ad = new IVRM_Master_ViddyBharthiDelegate();
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public ScholorshipMasterDTO Get( int id)
        {
            ScholorshipMasterDTO dto = new ScholorshipMasterDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));


            return ad.getalldetails(dto);
        }
        [Route("savecountry")]
        public ScholorshipMasterDTO savecountry([FromBody] ScholorshipMasterDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return ad.savecountry(dto);
        }
        [Route("savestate")]
        public ScholorshipStateDTO savestate([FromBody] ScholorshipStateDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return ad.savestate(dto);
        }
        [Route("onchnagestate")]
        public ScholorshipDitictDTO onchnagestate([FromBody] ScholorshipDitictDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return ad.onchnagestate(dto);
        }
        [Route("saveDistrict")]
        public ScholorshipDitictDTO saveDistrict([FromBody] ScholorshipDitictDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return ad.saveDistrict(dto);
        }
        [Route("savetaluka")]
        public ScholorshipTalukaDTO savetaluka([FromBody] ScholorshipTalukaDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return ad.savetaluka(dto);
        }
        [HttpPost]
        [Route("deactivateCountry")]
        public ScholorshipMasterDTO deactivateCountry([FromBody] ScholorshipMasterDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return ad.deactivateCountry(dto);
        }
        //deactivestate
        [Route("deactivestate")]
        public ScholorshipStateDTO deactivestate([FromBody] ScholorshipStateDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return ad.deactivestate(dto);
        }
        [Route("deactivedistict")]
        public ScholorshipDitictDTO deactivedistict([FromBody] ScholorshipDitictDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return ad.deactivedistict(dto);
        }
        [Route("deactivetaluka")]
        public ScholorshipTalukaDTO deactivetaluka([FromBody] ScholorshipTalukaDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return ad.deactivetaluka(dto);
        }
    }
}
