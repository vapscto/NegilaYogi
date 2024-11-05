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
    public class NaacFinanceSupport632Controller : Controller
    {
        NaacFinanceSupport632Delegate _delobj = new NaacFinanceSupport632Delegate();

        [Route("loaddata/{id:int}")]
        public NAAC_AC_632_FinanceSupport_DTO loaddata(int id)
        {
            NAAC_AC_632_FinanceSupport_DTO data = new NAAC_AC_632_FinanceSupport_DTO();
            data.MI_Id = id;
            data.UserId= Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delobj.loaddata(data);
        }
        [Route("save")]
        public NAAC_AC_632_FinanceSupport_DTO save([FromBody] NAAC_AC_632_FinanceSupport_DTO data)
        {
            
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delobj.save(data);
        }
        [Route("deactive")]
        public NAAC_AC_632_FinanceSupport_DTO deactive([FromBody] NAAC_AC_632_FinanceSupport_DTO data)
        {
            
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delobj.deactive(data);
        }
        [Route("EditData")]
        public NAAC_AC_632_FinanceSupport_DTO EditData([FromBody] NAAC_AC_632_FinanceSupport_DTO data)
        {
            
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delobj.EditData(data);
        }




        [Route("viewuploadflies")]
        public NAAC_AC_632_FinanceSupport_DTO viewuploadflies([FromBody] NAAC_AC_632_FinanceSupport_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.viewuploadflies(data);
        }


        [Route("deleteuploadfile")]
        public NAAC_AC_632_FinanceSupport_DTO deleteuploadfile([FromBody] NAAC_AC_632_FinanceSupport_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.deleteuploadfile(data);
        }



        [Route("savemedicaldatawisecomments")]
        public NAAC_AC_632_FinanceSupport_DTO savemedicaldatawisecomments([FromBody] NAAC_AC_632_FinanceSupport_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delobj.savemedicaldatawisecomments(data);
        }
        [Route("savefilewisecomments")]
        public NAAC_AC_632_FinanceSupport_DTO savefilewisecomments([FromBody] NAAC_AC_632_FinanceSupport_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delobj.savefilewisecomments(data);
        }
        [Route("getcomment")]
        public NAAC_AC_632_FinanceSupport_DTO getcomment([FromBody] NAAC_AC_632_FinanceSupport_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delobj.getcomment(data);
        }
        [Route("getfilecomment")]
        public NAAC_AC_632_FinanceSupport_DTO getfilecomment([FromBody] NAAC_AC_632_FinanceSupport_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delobj.getfilecomment(data);
        }

    }
}
