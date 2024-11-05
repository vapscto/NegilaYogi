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
    public class NaacMemberships423Controller : Controller
    {
        Naac_Memberships_423_Delegate _Delobj = new Naac_Memberships_423_Delegate();

        [Route("deactiveStudent")]
        public Naac_Memberships_423_DTO deactiveStudent([FromBody] Naac_Memberships_423_DTO data)
        {
           // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _Delobj.deactiveStudent(data);
        }

        [Route("save")]
        public Naac_Memberships_423_DTO save([FromBody] Naac_Memberships_423_DTO data)
        {
           
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _Delobj.save(data);
        }

        [Route("getcomment")]
        public Naac_Memberships_423_DTO getcomment([FromBody] Naac_Memberships_423_DTO data)
        {
           
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _Delobj.getcomment(data);
        }

        [Route("getfilecomment")]
        public Naac_Memberships_423_DTO getfilecomment([FromBody] Naac_Memberships_423_DTO data)
        {
           
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _Delobj.getfilecomment(data);
        }

        [Route("savemedicaldatawisecomments")]
        public Naac_Memberships_423_DTO savemedicaldatawisecomments([FromBody] Naac_Memberships_423_DTO data)
        {
           
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _Delobj.savemedicaldatawisecomments(data);
        }

        [Route("savefilewisecomments")]
        public Naac_Memberships_423_DTO savefilewisecomments([FromBody] Naac_Memberships_423_DTO data)
        {
           
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _Delobj.savefilewisecomments(data);
        }

        [Route("loaddata/{id:int}")]
        public Naac_Memberships_423_DTO loaddata(int id)
        {
            Naac_Memberships_423_DTO data = new Naac_Memberships_423_DTO();
            data.MI_Id = id;
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _Delobj.loaddata(data);
        }

        [Route("EditData")]
        public Naac_Memberships_423_DTO EditData([FromBody] Naac_Memberships_423_DTO data)
        {

            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _Delobj.EditData(data);
        }

        [Route("viewuploadflies")]
        public Naac_Memberships_423_DTO viewuploadflies([FromBody] Naac_Memberships_423_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _Delobj.viewuploadflies(data);
        }

        [Route("deleteuploadfile")]
        public Naac_Memberships_423_DTO deleteuploadfile([FromBody] Naac_Memberships_423_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _Delobj.deleteuploadfile(data);
        }
    }
}
