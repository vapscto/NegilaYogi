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
    public class NAAC_422_Clinical_LaboratoryController : Controller
    {
        public NAAC_422_Clinical_LaboratoryDelegate _objdel = new NAAC_422_Clinical_LaboratoryDelegate();

        [Route("loaddata/{id:int}")]
        public NAAC_MC_422_Clinical_Laboratory_DTO loaddata(int id)
        {
            NAAC_MC_422_Clinical_Laboratory_DTO data = new NAAC_MC_422_Clinical_Laboratory_DTO();          
            data.MI_Id = id;
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.loaddata(data);
        }

        [Route("savedata")]
        public NAAC_MC_422_Clinical_Laboratory_DTO savedata([FromBody]NAAC_MC_422_Clinical_Laboratory_DTO data)
        {

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.savedata(data);
        }

        [Route("editdata")]
        public NAAC_MC_422_Clinical_Laboratory_DTO editdata([FromBody]NAAC_MC_422_Clinical_Laboratory_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.editdata(data);
        }
        [Route("deactive_Y")]
        public NAAC_MC_422_Clinical_Laboratory_DTO deactive_Y([FromBody]NAAC_MC_422_Clinical_Laboratory_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.deactive_Y(data);
        }
        [Route("viewuploadflies")]
        public NAAC_MC_422_Clinical_Laboratory_DTO viewuploadflies([FromBody]NAAC_MC_422_Clinical_Laboratory_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.viewuploadflies(data);
        }
        [Route("deleteuploadfile")]
        public NAAC_MC_422_Clinical_Laboratory_DTO deleteuploadfile([FromBody]NAAC_MC_422_Clinical_Laboratory_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.deleteuploadfile(data);
        }


    }
}
