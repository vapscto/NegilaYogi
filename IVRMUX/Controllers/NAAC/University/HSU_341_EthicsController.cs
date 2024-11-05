using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.NAAC.University;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.NAAC.University;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.NAAC.University
{
    [Route("api/[controller]")]
    public class HSU_341_EthicsController : Controller
    {
        HSU_341_EthicsDelegate del = new HSU_341_EthicsDelegate();
        [Route("loaddata/{id:int}")]
        public HSU_341_EthicsDTO loaddata(int id)
        {
            HSU_341_EthicsDTO data = new HSU_341_EthicsDTO();

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = id;
            return del.loaddata(data);
        }
        [Route("savedata")]
        public HSU_341_EthicsDTO savedata([FromBody] HSU_341_EthicsDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savedata(data);
        }
        [Route("deactive")]
        public HSU_341_EthicsDTO deactive([FromBody] HSU_341_EthicsDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.deactive(data);
        }
        [Route("editdata")]
        public HSU_341_EthicsDTO editdata([FromBody] HSU_341_EthicsDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.editdata(data);
        }
    }
}
