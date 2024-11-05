using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Transport;
using corewebapi18072016.Delegates.com.vapstech.Transport;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Transport
{
    [Route("api/[controller]")]
    public class TripController : Controller
    {
        TripDelegate dell = new TripDelegate();
        [Route("loadData1/{id:int}")]
        public TripDTO loadData1(int id)
        {
            TripDTO dto = new TripDTO();
            var session = HttpContext.Session.GetString("Session_Trip_IsOnline");
            if (session != null)
            {
                if (session.Equals("Online"))
                {
                    dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_Trip_MI_Id"));
                }
            }
            else
            {
                dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            }
            return dell.getdata(dto);
        }




        [Route("getvahicle")]
        public TripDTO getvahicle([FromBody] TripDTO data)
        {
            var session = HttpContext.Session.GetString("Session_Trip_IsOnline");
            if (session != null)
            {
                if (session.Equals("Online"))
                {
                    data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_Trip_MI_Id"));
                }
            }
            else
            {
                data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            }
            return dell.getvahicle(data);
        }


        [Route("duprecpcheck")]
        public TripDTO duprecpcheck([FromBody] TripDTO data)
        {
            var session = HttpContext.Session.GetString("Session_Trip_IsOnline");
            if (session != null)
            {
                if (session.Equals("Online"))
                {
                    data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_Trip_MI_Id"));
                }
            }
            else
            {
                data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            }
            return dell.duprecpcheck(data);
        }






        [Route("Search")]
        public TripDTO Search([FromBody] TripDTO data)
        {
            var session = HttpContext.Session.GetString("Session_Trip_IsOnline");
            if (session != null)
            {
                if (session.Equals("Online"))
                {
                    data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_Trip_MI_Id"));
                }
            }
            else
            {
                data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            }
            return dell.search(data);
        }
        [Route("save")]
        public TripDTO save([FromBody] TripDTO data)
        {
            var session = HttpContext.Session.GetString("Session_Trip_IsOnline");
            if (session != null)
            {
                if (session.Equals("Online"))
                {
                    data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_Trip_MI_Id"));
                    data.asmay_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_Trip_ASMAY_Id"));
                }
            }
            else
            {
                data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
                data.asmay_id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            }
            return dell.savedata(data);
        }
        [Route("SearchByTripId")]
        public TripDTO SearchByTripId([FromBody]TripDTO dto)
        {
            var session = HttpContext.Session.GetString("Session_Trip_IsOnline");
            if (session != null)
            {
                if (session.Equals("Online"))
                {
                    dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_Trip_MI_Id"));
                    dto.asmay_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_Trip_ASMAY_Id"));
                }
            }
            else
            {
                dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
                dto.asmay_id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            }
            return dell.SearchByTripId(dto);
        }
        [Route("getbillNo")]
        public TripDTO getbillNo([FromBody]TripDTO obj)
        {
            var session = HttpContext.Session.GetString("Session_Trip_IsOnline");
            if (session != null)
            {
                if (session.Equals("Online"))
                {
                    obj.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_Trip_MI_Id"));
                    obj.asmay_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_Trip_ASMAY_Id"));
                }
            }
            else
            {
                obj.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
                obj.asmay_id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            }
            return dell.getbillNo(obj);
        }
        [Route("pay")]
        public TripDTO pay([FromBody]TripDTO data)
        {
            var session = HttpContext.Session.GetString("Session_Trip_IsOnline");
            if (session != null)
            {
                if (session.Equals("Online"))
                {
                    data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_Trip_MI_Id"));
                    data.asmay_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_Trip_ASMAY_Id"));
                }
            }
            else
            {
                data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
                data.asmay_id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            }
            return dell.pay(data);
        }
        [Route("GetTripDetails/{id:int}")]
        public TripDTO GetTripDetails(int id)
        {
            TripDTO dto = new TripDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return dell.GetTripDetails(dto);
        }
        [Route("approveTrip/{id:int}")]
        public TripDTO approveTrip(int id)
        {
            TripDTO dto = new TripDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.TRTP_Id = id;
            return dell.approve(dto);
        }
        [Route("rejectTrip/{id:int}")]
        public TripDTO rejectTrip(int id)
        {
            TripDTO dto = new TripDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.TRTP_Id = id;
            return dell.reject(dto);
        }
        [Route("viewDetails/{id:int}")]
        public TripDTO viewDetails(int id)
        {
            TripDTO data = new TripDTO();
            data.TRTP_Id = id;
            var session = HttpContext.Session.GetString("Session_Trip_IsOnline");
            if (session != null)
            {
                if (session.Equals("Online"))
                {
                    data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_Trip_MI_Id"));
                    data.asmay_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_Trip_ASMAY_Id"));
                }
            }
            else
            {
                data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
                data.asmay_id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            }
            return dell.viewDetails(data);
        }

        //praveen
        [Route("printrecept")]
        public TripDTO printrecept([FromBody]TripDTO obj)
        {
            var session = HttpContext.Session.GetString("Session_Trip_IsOnline");
            if (session != null)
            {
                if (session.Equals("Online"))
                {
                    obj.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_Trip_MI_Id"));
                    obj.asmay_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_Trip_ASMAY_Id"));
                }
            }
            else
            {
                obj.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
                obj.asmay_id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            }
            return dell.printrecept(obj);
        }

        [Route("printbill")]
        public TripDTO printbill([FromBody]TripDTO obj)
        {
            var session = HttpContext.Session.GetString("Session_Trip_IsOnline");
            if (session != null)
            {
                if (session.Equals("Online"))
                {
                    obj.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_Trip_MI_Id"));
                    obj.asmay_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_Trip_ASMAY_Id"));
                }
            }
            else
            {
                obj.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
                obj.asmay_id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            }
            return dell.printbill(obj);
        }
        

        [Route("printtripsheet")]
        public TripDTO printtripsheet([FromBody]TripDTO obj)
        {
            var session = HttpContext.Session.GetString("Session_Trip_IsOnline");
            if (session != null)
            {
                if (session.Equals("Online"))
                {
                    obj.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_Trip_MI_Id"));
                    obj.asmay_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_Trip_ASMAY_Id"));
                }
            }
            else
            {
                obj.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
                obj.asmay_id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            }
            return dell.printtripsheet(obj);
        }
        [Route("deletetrip")]
        public TripDTO deletetrip([FromBody]TripDTO obj)
        {
            var session = HttpContext.Session.GetString("Session_Trip_IsOnline");
            if (session != null)
            {
                if (session.Equals("Online"))
                {
                    obj.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_Trip_MI_Id"));
                    obj.asmay_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_Trip_ASMAY_Id"));
                }
            }
            else
            {
                obj.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
                obj.asmay_id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            }
            return dell.deletetrip(obj);
        }
        
    }
}
