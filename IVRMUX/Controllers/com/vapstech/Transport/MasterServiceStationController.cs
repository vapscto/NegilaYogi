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
    public class MasterServiceStationController : Controller
    {
        MasterServiceStationDelegate _delegate = new MasterServiceStationDelegate();
        //parts report
        [Route("getrptparam/{id:int}")]
        public ServiceStationDTO getrptparam(int id)
        {
            ServiceStationDTO obj = new ServiceStationDTO();
            obj.MI_Id= Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getrptparam(obj);
        }
        [Route("Getreportdetails")]
        public ServiceStationDTO Getreportdetails([FromBody]ServiceStationDTO data )
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.Getreportdetails(data);
        }

        //SERVICE Bill

        
             [Route("Servicebillload/{id:int}")]
        public ServiceStationDTO Servicebillload(int id)
        {
            ServiceStationDTO obj = new ServiceStationDTO();
            obj.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.Servicebillload(obj);
        }
        [Route("get_srvdetails")]
        public ServiceStationDTO get_srvdetails([FromBody] ServiceStationDTO obj)
        {
            obj.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            obj.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delegate.get_srvdetails(obj);
        }
        [Route("saveBilldata")]
        public ServiceStationDTO saveBilldata([FromBody] ServiceStationDTO obj)
        {
            obj.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            obj.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            obj.USER_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.saveBilldata(obj);
        }

        [Route("PayBill")]
        public ServiceStationDTO PayBill([FromBody] ServiceStationDTO obj)
        {
            obj.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            obj.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            obj.USER_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.PayBill(obj);
        }

        [Route("FinalPayBill")]
        public ServiceStationDTO FinalPayBill([FromBody] ServiceStationDTO obj)
        {
            obj.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            obj.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            obj.USER_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.FinalPayBill(obj);
        }
        [Route("findservice")]
        public ServiceStationDTO findservice([FromBody] ServiceStationDTO obj)
        {
            obj.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            obj.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            obj.USER_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.findservice(obj);
        }
        [Route("duprecpcheck")]
        public ServiceStationDTO duprecpcheck([FromBody] ServiceStationDTO obj)
        {
            obj.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            obj.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            obj.USER_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.duprecpcheck(obj);
        }

        [Route("delete_rec")]
        public ServiceStationDTO delete_rec([FromBody] ServiceStationDTO obj)
        {
            obj.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            obj.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            obj.USER_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.delete_rec(obj);
        }

        //SERVICE BILL END




        //Master Parts.

        [Route("getpartsdata/{id:int}")]
        public ServiceStationDTO getpartsdata(int id)
        {
            ServiceStationDTO obj = new ServiceStationDTO();
            obj.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getpartsdata(obj);
        }
        [Route("savepartsdata")]
        public ServiceStationDTO savepartsdata([FromBody]ServiceStationDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.USER_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.savepartsdata(data);
        }
        [Route("editpartsdata/{id:int}")]
        public ServiceStationDTO editpartsdata(int id)
        {
            return _delegate.editpartsdata(id);
        }

        [Route("activedeactiveparts")]
        public ServiceStationDTO activedeactiveparts([FromBody] ServiceStationDTO data)
        {
            return _delegate.activedeactiveparts(data);
        }
        [Route("viewreq")]
        public ServiceStationDTO viewreq([FromBody] ServiceStationDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.USER_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));

            return _delegate.viewreq(data);
        }
        [Route("getbillreport")]
        public ServiceStationDTO getbillreport([FromBody] ServiceStationDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.USER_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));

            return _delegate.getbillreport(data);
        }

       
        //Master service station.
        [Route("loadservicestation/{id:int}")]
        public ServiceStationDTO loadservicestation(int id)
        {
            ServiceStationDTO obj = new ServiceStationDTO();
            obj.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
          
            return _delegate.loadservicestation(obj);
        }
        [Route("savestation")]
        public ServiceStationDTO savestation([FromBody]ServiceStationDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.USER_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.savestation(data);
        }


        [Route("viewitems/{id:int}")]
        public ServiceStationDTO viewitems(int id)
        {
            return _delegate.viewitems(id);
        }
        [Route("Editstation/{id:int}")]
        public ServiceStationDTO Editstation(int id)
        {
            return _delegate.Editstation(id);
        }
        [Route("deactivatestation")]
        public ServiceStationDTO deactivatestation([FromBody]ServiceStationDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
          //  data.TRMH_Id = data.TRMH_Id;
            return _delegate.deactivatestation(data);
        }

        //Master partperticulartype
        [Route("loadparttype/{id:int}")]
        public ServiceStationDTO loadparttype(int id)
        {
            ServiceStationDTO obj = new ServiceStationDTO();
            obj.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.loadparttype(obj);
        }
        [Route("saveparttype")]
        public ServiceStationDTO saveparttype([FromBody]ServiceStationDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.USER_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.saveparttype(data);
        }
        [Route("Editparttype/{id:int}")]
        public ServiceStationDTO Editparttype(int id)
        {
            return _delegate.Editparttype(id);
        }
        [Route("deactivateparttype")]
        public ServiceStationDTO deactivateparttype([FromBody]ServiceStationDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //  data.TRMH_Id = data.TRMH_Id;
            return _delegate.deactivateparttype(data);
        }
    }
}
