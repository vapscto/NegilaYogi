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
    public class NaacExpenditure424Controller : Controller
    {
        NaacExpenditure424Delegate _Delobj = new NaacExpenditure424Delegate();

        [Route("save")]
        public NaacExpenditure424DTO save([FromBody]NaacExpenditure424DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _Delobj.save(data);
        }

        [Route("loaddata/{id:int}")]
        public NaacExpenditure424DTO loaddata(int id)
        {
            NaacExpenditure424DTO data = new NaacExpenditure424DTO();
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = id;
            return _Delobj.loaddata(data);
        }

        [Route("deactiveStudent")]
        public NaacExpenditure424DTO deactiveStudent([FromBody] NaacExpenditure424DTO data)
        {

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _Delobj.deactiveStudent(data);
        }

        [Route("getcomment")]
        public NaacExpenditure424DTO getcomment([FromBody] NaacExpenditure424DTO data)
        {

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _Delobj.getcomment(data);
        }

        [Route("getfilecomment")]
        public NaacExpenditure424DTO getfilecomment([FromBody] NaacExpenditure424DTO data)
        {

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _Delobj.getfilecomment(data);
        }

        [Route("savemedicaldatawisecomments")]
        public NaacExpenditure424DTO savemedicaldatawisecomments([FromBody] NaacExpenditure424DTO data)
        {

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _Delobj.savemedicaldatawisecomments(data);
        }

        [Route("savefilewisecomments")]
        public NaacExpenditure424DTO savefilewisecomments([FromBody] NaacExpenditure424DTO data)
        {

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _Delobj.savefilewisecomments(data);
        }

        [Route("EditData")]
        public NaacExpenditure424DTO EditData([FromBody] NaacExpenditure424DTO data)
        {

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _Delobj.EditData(data);
        }

        [Route("viewuploadflies")]
        public NaacExpenditure424DTO viewuploadflies([FromBody] NaacExpenditure424DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _Delobj.viewuploadflies(data);
        }

        [Route("deleteuploadfile")]
        public NaacExpenditure424DTO deleteuploadfile([FromBody] NaacExpenditure424DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _Delobj.deleteuploadfile(data);
        }

    }
}
