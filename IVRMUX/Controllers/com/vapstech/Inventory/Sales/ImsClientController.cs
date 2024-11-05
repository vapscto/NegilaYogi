using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Inventory.Sales;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory;

namespace IVRMUX.Controllers.com.vapstech.Inventory.Sales
{
    [Produces("application/json")]
    [Route("api/ImsClient")]
    public class ImsClientController : Controller
    {
        public ImsClientDelegate _objdel = new ImsClientDelegate();

        [Route("getdetails/{id:int}")]
        public Clients_DTO getdetails(int id)
        {
            Clients_DTO data = new Clients_DTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.getdetails(data);
        }

        [Route("OnChangeTab1Inst")]
        public Clients_DTO OnChangeTab1Inst([FromBody]Clients_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.OnChangeTab1Inst(data);
        }

        [Route("saveClientdata")]
        public Clients_DTO saveClientdata([FromBody]Clients_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.saveClientdata(data);
        }

        [Route("clientDecative")]
        public Clients_DTO clientDecative([FromBody]Clients_DTO data)
        {
            return _objdel.clientDecative(data);
        }

        [Route("editClientdata")]
        public Clients_DTO editClientdata([FromBody]Clients_DTO data)
        {
            return _objdel.editClientdata(data);
        }

        [Route("OnChangeTab2Inst")]
        public Clients_DTO OnChangeTab2Inst([FromBody]Clients_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.OnChangeTab2Inst(data);
        }

        [Route("saveClientMappingdata")]
        public Clients_DTO saveClientMappingdata([FromBody]Clients_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.saveClientMappingdata(data);
        }

        [Route("editClientMappingdata")]
        public Clients_DTO editClientMappingdata([FromBody]Clients_DTO data)
        {
            return _objdel.editClientMappingdata(data);
        }

        [Route("deactiveClientMappingdata")]
        public Clients_DTO deactiveClientMappingdata([FromBody]Clients_DTO data)
        {
            return _objdel.deactiveClientMappingdata(data);
        }

        [Route("modalListdata")]
        public Clients_DTO modalListdata([FromBody]Clients_DTO data)
        {
            return _objdel.modalListdata(data);
        }

        //VMS Client And IVRM Client Mapping 
        [Route("OnChangeClientTab3")]
        public Clients_DTO OnChangeClientTab3([FromBody]Clients_DTO data)
        {
            return _objdel.OnChangeClientTab3(data);
        }

        [Route("SaveVMSIVRMMapping")]
        public Clients_DTO SaveVMSIVRMMapping([FromBody]Clients_DTO data)
        {
            return _objdel.SaveVMSIVRMMapping(data);
        }
    }
}