using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.admission;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.admission
{
    [Route("api/[controller]")]
    public class Adm_School_Master_CEController : Controller
    {
        Adm_School_Master_CEDelegate del = new Adm_School_Master_CEDelegate();

        [Route("getdata/{id:int}")]

        public Adm_School_Master_CE_DTO getdata(int id)
        {
            Adm_School_Master_CE_DTO data = new Adm_School_Master_CE_DTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getdata(data);
        }
        [Route("savedata")]

        public Adm_School_Master_CE_DTO savedata([FromBody] Adm_School_Master_CE_DTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.savedata(data);
        }
        [Route("savedata2")]

        public Adm_School_Master_CE_DTO savedata2([FromBody] Adm_School_Master_CE_DTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.savedata2(data);
        }
        [Route("editdata")]

        public Adm_School_Master_CE_DTO editdata([FromBody] Adm_School_Master_CE_DTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.editdata(data);
        }
        [Route("activedeactive")]

        public Adm_School_Master_CE_DTO activedeactive([FromBody] Adm_School_Master_CE_DTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.activedeactive(data);
        }
       
        [Route("deactive2")]

        public Adm_School_Master_CE_DTO deactive2([FromBody] Adm_School_Master_CE_DTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.deactive2(data);
        }
        [Route("edit2")]

        public Adm_School_Master_CE_DTO edit2([FromBody] Adm_School_Master_CE_DTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.edit2(data);
        }

    }
}
