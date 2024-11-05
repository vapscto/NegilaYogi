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
    public class MC_819_Accredition_ClinicallabController : Controller
    {
        public MC_819_Accredition_ClinicallabDelegate _objdel = new MC_819_Accredition_ClinicallabDelegate();


        [Route("loaddata/{id:int}")]
        public MC_819_Accredition_ClinicallabDTO loaddata(int id)
        {
            MC_819_Accredition_ClinicallabDTO data = new MC_819_Accredition_ClinicallabDTO();
            data.MI_Id = id;
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.loaddata(data);
        }

        [Route("savedata")]
        public MC_819_Accredition_ClinicallabDTO savedata([FromBody]MC_819_Accredition_ClinicallabDTO data)
        {

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.savedata(data);
        }
         [Route("savedata1")]
        public MC_819_Accredition_ClinicallabDTO savedata1([FromBody]MC_819_Accredition_ClinicallabDTO data)
        {

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.savedata1(data);
        }
        [Route("savedata2")]
        public MC_819_Accredition_ClinicallabDTO savedata2([FromBody]MC_819_Accredition_ClinicallabDTO data)
        {

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.savedata2(data);
        }
        [Route("savedata3")]
        public MC_819_Accredition_ClinicallabDTO savedata3([FromBody]MC_819_Accredition_ClinicallabDTO data)
        {

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.savedata3(data);
        }

        [Route("editdata")]
        public MC_819_Accredition_ClinicallabDTO editdata([FromBody]MC_819_Accredition_ClinicallabDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.editdata(data);
        }
        [Route("deactivate")]
        public MC_819_Accredition_ClinicallabDTO deactivate([FromBody]MC_819_Accredition_ClinicallabDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.deactivate(data);
        }
        [Route("getcomment")]
        public MC_819_Accredition_ClinicallabDTO getcomment([FromBody]MC_819_Accredition_ClinicallabDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.getcomment(data);
        }
        [Route("savecomments")]
        public MC_819_Accredition_ClinicallabDTO savecomments([FromBody]MC_819_Accredition_ClinicallabDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.savecomments(data);
        }

    }
}
