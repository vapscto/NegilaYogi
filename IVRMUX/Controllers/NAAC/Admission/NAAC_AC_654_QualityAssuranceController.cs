using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.NAAC.Admission;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.NAAC.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.NAAC.Admission
{
    [Route("api/[controller]")]
    public class NAAC_AC_654_QualityAssuranceController : Controller
    {
        NAAC_AC_654_QualityAssuranceDelegate _delobj = new NAAC_AC_654_QualityAssuranceDelegate();
        [Route("loaddata/{id:int}")]
        public NAAC_Criteria_6_DTO loaddata(int id)
        {
            NAAC_Criteria_6_DTO data = new NAAC_Criteria_6_DTO();
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = id;
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delobj.loaddata(data);
        }
        [Route("save")]
        public NAAC_Criteria_6_DTO save([FromBody]NAAC_Criteria_6_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.save(data);
        }
        [Route("deactiveStudent")]
        public NAAC_Criteria_6_DTO deactiveStudent([FromBody] NAAC_Criteria_6_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
           // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.deactiveStudent(data);
        }
        [Route("EditData")]
        public NAAC_Criteria_6_DTO EditData([FromBody] NAAC_Criteria_6_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.EditData(data);
        }

        [Route("viewuploadflies")]
        public NAAC_Criteria_6_DTO viewuploadflies([FromBody] NAAC_Criteria_6_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
           // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.viewuploadflies(data);
        }
        [Route("deleteuploadfile")]
        public NAAC_Criteria_6_DTO deleteuploadfile([FromBody] NAAC_Criteria_6_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.deleteuploadfile(data);
        }



        [Route("savemedicaldatawisecomments")]
        public NAAC_Criteria_6_DTO savemedicaldatawisecomments([FromBody] NAAC_Criteria_6_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delobj.savemedicaldatawisecomments(data);
        }
        [Route("savefilewisecomments")]
        public NAAC_Criteria_6_DTO savefilewisecomments([FromBody] NAAC_Criteria_6_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delobj.savefilewisecomments(data);
        }
        [Route("getcomment")]
        public NAAC_Criteria_6_DTO getcomment([FromBody] NAAC_Criteria_6_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delobj.getcomment(data);
        }
        [Route("getfilecomment")]
        public NAAC_Criteria_6_DTO getfilecomment([FromBody] NAAC_Criteria_6_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delobj.getfilecomment(data);
        }


    }
}
