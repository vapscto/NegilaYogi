using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Sports;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Sports;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Sports
{
    [Route("api/[controller]")]
    public class HouseInchargeController : Controller
    {
        // GET: api/<controller>

        private HouseInchargeDelegate _objdel = new HouseInchargeDelegate();


        [Route("Getdetails/{id:int}")]
        public SPCC_Master_House_Staff_DTO Getdetails(int id)
        {
            SPCC_Master_House_Staff_DTO dto = new SPCC_Master_House_Staff_DTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.Getdetails(dto);
        }

        [Route("saverecord")]
        public SPCC_Master_House_Staff_DTO saverecord([FromBody] SPCC_Master_House_Staff_DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.saverecord(dto);
        }

        [Route("editrecord")]
        public SPCC_Master_House_Staff_DTO editrecord([FromBody] SPCC_Master_House_Staff_DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.editrecord(dto);
        }
        [Route("get_House")]
        public SPCC_Master_House_Staff_DTO get_House([FromBody] SPCC_Master_House_Staff_DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.get_House(dto);
        }
        [Route("deactive")]
        public SPCC_Master_House_Staff_DTO deactive([FromBody] SPCC_Master_House_Staff_DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.deactive(dto);
        }

        [Route("getdepchange")]
        public SPCC_Master_House_Staff_DTO getdepchange([FromBody] SPCC_Master_House_Staff_DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.getdepchange(dto);
        }
        [Route("get_staff1")]
        public SPCC_Master_House_Staff_DTO get_staff1([FromBody] SPCC_Master_House_Staff_DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.get_staff1(dto);
        }
        
    }
}
