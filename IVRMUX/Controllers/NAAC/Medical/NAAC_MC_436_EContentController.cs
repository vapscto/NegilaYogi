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
    public class NAAC_MC_436_EContentController : Controller
    {
        public NAAC_MC_436_EContentDelegate _Delobj = new NAAC_MC_436_EContentDelegate();

        [Route("loaddata/{id:int}")]
        public NAAC_MC_436_EContent_DTO loaddata(int id)
        {
            NAAC_MC_436_EContent_DTO data = new NAAC_MC_436_EContent_DTO();
            data.MI_Id = id;
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _Delobj.loaddata(data);
        }

        [Route("savedata")]
        public NAAC_MC_436_EContent_DTO savedata([FromBody]NAAC_MC_436_EContent_DTO data)
        {

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _Delobj.savedata(data);
        }

        [Route("editdata")]
        public NAAC_MC_436_EContent_DTO editdata([FromBody]NAAC_MC_436_EContent_DTO data)
        {

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _Delobj.editdata(data);
        }

        [Route("deactiveStudent")]
        public NAAC_MC_436_EContent_DTO deactiveStudent([FromBody]NAAC_MC_436_EContent_DTO data)
        {

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _Delobj.deactiveStudent(data);
        }

        [Route("viewuploadflies")]
        public NAAC_MC_436_EContent_DTO viewuploadflies([FromBody] NAAC_MC_436_EContent_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return _Delobj.viewuploadflies(data);
        }

        [Route("deleteuploadfile")]
        public NAAC_MC_436_EContent_DTO deleteuploadfile([FromBody] NAAC_MC_436_EContent_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return _Delobj.deleteuploadfile(data);
        }
    }
}
