using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.Transport;
using PreadmissionDTOs.com.vaps.Transport;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Transport
{
    [Route("api/[controller]")]
    public class TripOnlineBookingController : Controller
    {
        TripOnlineBookingDelegate _dell = new TripOnlineBookingDelegate();
        [Route("loadData/{id:int}")]
        public TripOnlineBookingDTO loadData(int id)
        {
            TripOnlineBookingDTO dto = new TripOnlineBookingDTO();
            dto.MI_Id= Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.asmay_id= Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            //var mi_Id = id.ToString();
            //if (mi_Id != null)
            //{
            //    HttpContext.Session.SetString("Session_Trip_IsOnline", "Online");
            //    dto.IsOnline = HttpContext.Session.GetString("Session_Trip_IsOnline");

            //    HttpContext.Session.SetInt32("Session_Trip_MI_Id", Convert.ToInt32(mi_Id));
            //   dto.MI_Id= Convert.ToInt32(HttpContext.Session.GetInt32("Session_Trip_MI_Id"));

            //}
            //var sessiondata=_dell.setsessionvalue(dto);
            //if (sessiondata.asmay_id > 0)
            //{
            //    HttpContext.Session.SetInt32("Session_Trip_ASMAY_Id", Convert.ToInt32(sessiondata.asmay_id));
            //    dto.asmay_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_Trip_ASMAY_Id"));
            //}
            return _dell.load(dto);
        }
        [Route("save")]
        public TripOnlineBookingDTO save([FromBody] TripOnlineBookingDTO data)
        {
            // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_Trip_MI_Id"));
            // data.asmay_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_Trip_ASMAY_Id"));
           data.MI_Id= Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
           data.asmay_id =Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _dell.save(data);
        }
        [Route("getHirer/{id:int}")]
        public TripOnlineBookingDTO getHirer(int id)
        {
            TripOnlineBookingDTO dto = new TripOnlineBookingDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.TRHG_Id = id;
            return _dell.getHirer(dto);
        }
        [Route("getHirerDetails")]
        public TripOnlineBookingDTO getHirerDetails([FromBody]TripOnlineBookingDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _dell.getHirerDetails(dto);
        }
        [Route("edit/{id:int}")]
        public TripOnlineBookingDTO edit(int id)
        {
            return _dell.edit(id);
        }
        [Route("deactivate")]
        public TripOnlineBookingDTO deactivate([FromBody]TripOnlineBookingDTO obj)
        {
            return _dell.deactivate(obj);
        }
    }
}
