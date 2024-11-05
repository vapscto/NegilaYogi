using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.NAAC.Medical;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.NAAC.Medical;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.NAAC.Medical
{
    [Route("api/[controller]")]
    public class MC_Programs_112Controller : Controller
    {

        public MC_Programs_112Delegate _objdel = new MC_Programs_112Delegate();

        [Route("loaddata/{id:int}")]
        public MC_Programs_112_DTO loaddata(int id)
        {
            MC_Programs_112_DTO data = new MC_Programs_112_DTO();
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = id;
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.loaddata(data);
        }

        [Route("savedata")]
        public MC_Programs_112_DTO savedata([FromBody]MC_Programs_112_DTO data)
        {

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.savedata(data);
        }

        [Route("editdata")]
        public MC_Programs_112_DTO editdata([FromBody]MC_Programs_112_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.editdata(data);
        }
        [Route("deactive_Y")]
        public MC_Programs_112_DTO deactive_Y([FromBody]MC_Programs_112_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.deactive_Y(data);
        }
        [Route("viewuploadflies")]
        public MC_Programs_112_DTO viewuploadflies([FromBody]MC_Programs_112_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.viewuploadflies(data);
        }
        [Route("deleteuploadfile")]
        public MC_Programs_112_DTO deleteuploadfile([FromBody]MC_Programs_112_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.deleteuploadfile(data);
        }
        [Route("StaffList_Boss")]
        public MC_Programs_112_DTO StaffList_Boss([FromBody]MC_Programs_112_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.StaffList_Boss(data);
        }
        [Route("StaffList_Council")]
        public MC_Programs_112_DTO StaffList_Council([FromBody]MC_Programs_112_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.StaffList_Council(data);
        }
    }
}
