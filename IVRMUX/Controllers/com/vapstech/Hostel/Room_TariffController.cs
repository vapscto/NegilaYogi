using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Hostel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Hostel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Hostel
{
    [Route("api/[controller]")]
    public class Room_TariffController : Controller
    {
        public Room_TariffDelegate _delObj = new Room_TariffDelegate();
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("loaddata/{id:int}")]
        public Room_Tariff_DTO loaddata(int id)
        {
            Room_Tariff_DTO data = new Room_Tariff_DTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delObj.loaddata(data);
        }
        
        [Route("savedata")]
        public Room_Tariff_DTO savedata([FromBody] Room_Tariff_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delObj.savedata(data);
        }

        [Route("editdata")]
        public Room_Tariff_DTO editdata([FromBody]Room_Tariff_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delObj.editdata(data);
        }

        [Route("Ydeactive")]
        public Room_Tariff_DTO Ydeactive([FromBody]Room_Tariff_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delObj.Ydeactive(data);
        }

        [Route("get_bedcount")]
        public Room_Tariff_DTO get_bedcount([FromBody]Room_Tariff_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delObj.get_bedcount(data);
        }

    }
}
