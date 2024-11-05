using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Transport;
using TransportServiceHub.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TransportServiceHub.Controllers
{
    [Route("api/[controller]")]
    public class MasterServiceStationFacade : Controller
    {
        public MasterServiceStationInterface _interface;
        public MasterServiceStationFacade(MasterServiceStationInterface inter)
        {
            _interface = inter;
        }
     
       [Route("getrptparam")]
       public ServiceStationDTO getrptparam([FromBody] ServiceStationDTO obj)
        {
            return _interface.getrptparam(obj);
        }
        [Route("Getreportdetails")]
        public ServiceStationDTO Getreportdetails([FromBody] ServiceStationDTO dt)
        {
            return _interface.Getreportdetails(dt);
        }
        //bill
        [Route("Servicebillload")]
        public ServiceStationDTO Servicebillload([FromBody] ServiceStationDTO obj)
        {
            return _interface.Servicebillload(obj);
        }
        [Route("get_srvdetails")]
        public ServiceStationDTO get_srvdetails([FromBody] ServiceStationDTO obj)
        {
            return _interface.get_srvdetails(obj);
        }
        [Route("saveBilldata")]
        public ServiceStationDTO saveBilldata([FromBody] ServiceStationDTO obj)
        {
            return _interface.saveBilldata(obj);
        }
        [Route("PayBill")]
        public ServiceStationDTO PayBill([FromBody] ServiceStationDTO obj)
        {
            return _interface.PayBill(obj);
        }
        [Route("FinalPayBill")]
        public ServiceStationDTO FinalPayBill([FromBody] ServiceStationDTO obj)
        {
            return _interface.FinalPayBill(obj);
        }
        [Route("findservice")]
        public ServiceStationDTO findservice([FromBody] ServiceStationDTO obj)
        {
            return _interface.findservice(obj);
        }
        [Route("duprecpcheck")]
        public ServiceStationDTO duprecpcheck([FromBody] ServiceStationDTO obj)
        {
            return _interface.duprecpcheck(obj);
        }
        [Route("delete_rec")]
        public ServiceStationDTO delete_rec([FromBody] ServiceStationDTO obj)
        {
            return _interface.delete_rec(obj);
        }


        //end bill




        //Master Hirer Rate.
        [Route("getpartsdata")]
        public ServiceStationDTO getpartsdata([FromBody] ServiceStationDTO obj)
        {
            return _interface.getpartsdata(obj);
        }
        [Route("savepartsdata")]
        public ServiceStationDTO savepartsdata([FromBody] ServiceStationDTO dt)
        {
            return _interface.savepartsdata(dt);
        }
      
        

        [Route("activedeactiveparts")]
        public ServiceStationDTO activedeactiveparts([FromBody] ServiceStationDTO dt)
        {
            return _interface.activedeactiveparts(dt);
        }
        [Route("viewreq")]
        public ServiceStationDTO viewreq([FromBody] ServiceStationDTO dt)
        {
            return _interface.viewreq(dt);
        }
        [Route("getbillreport")]
        public ServiceStationDTO getbillreport([FromBody] ServiceStationDTO dt)
        {
            return _interface.getbillreport(dt);
        }
        [Route("editpartsdata/{id:int}")]
        public ServiceStationDTO editpartsdata(int id)
        {
            return _interface.editpartsdata(id);
        }
        [Route("viewitems/{id:int}")]
        public ServiceStationDTO viewitems(int id)
        {
            return _interface.viewitems(id);
        }
    
      //Master service.
      [Route("loadservicestation")]
        public ServiceStationDTO loadservicestation([FromBody] ServiceStationDTO obj)
        {
            return _interface.loadservicestation(obj);
        }
        [Route("savestation")]
        public ServiceStationDTO savestation([FromBody] ServiceStationDTO dt)
        {
            return _interface.savestation(dt);
        }
        [Route("Editstation/{id:int}")]
        public ServiceStationDTO Editstation(int id)
        {
            return _interface.Editstation(id);
        }
        [Route("deactivatestation")]
        public ServiceStationDTO deactivatestation([FromBody]ServiceStationDTO dto)
        {
            return _interface.deactivatestation(dto);
        }

        //Master parttype.
        [Route("loadparttype")]
        public ServiceStationDTO loadparttype([FromBody] ServiceStationDTO obj)
        {
            return _interface.loadparttype(obj);
        }
        [Route("saveparttype")]
        public ServiceStationDTO saveparttype([FromBody] ServiceStationDTO dt)
        {
            return _interface.saveparttype(dt);
        }
        [Route("Editparttype/{id:int}")]
        public ServiceStationDTO Editparttype(int id)
        {
            return _interface.Editparttype(id);
        }
        [Route("deactivateparttype")]
        public ServiceStationDTO deactivateparttype([FromBody]ServiceStationDTO dto)
        {
            return _interface.deactivateparttype(dto);
        }
    }
}
