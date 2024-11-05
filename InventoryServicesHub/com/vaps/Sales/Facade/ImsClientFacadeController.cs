using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryServicesHub.com.vaps.Sales.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InventoryServicesHub.com.vaps.Sales.Facade
{
    [Route("api/[controller]")]
    public class ImsClientFacadeController : Controller
    {
        public ImsClientInterface _objinter;

        public ImsClientFacadeController(ImsClientInterface parameter)
        {
            _objinter = parameter;
        }

        [Route("getdetails")]
        public Clients_DTO getdetails([FromBody] Clients_DTO data)
        {
            return _objinter.getdetails(data);
        }

        [Route("OnChangeTab1Inst")]
        public Clients_DTO OnChangeTab1Inst([FromBody]Clients_DTO data)
        {
            return _objinter.OnChangeTab1Inst(data);
        }

        [Route("saveClientdata")]
        public Clients_DTO saveClientdata([FromBody]Clients_DTO data)
        {
            return _objinter.saveClientdata(data);
        }

        [Route("clientDecative")]
        public Clients_DTO clientDecative([FromBody]Clients_DTO data)
        {
            return _objinter.clientDecative(data);
        }

        [Route("editClientdata")]
        public Clients_DTO editClientdata([FromBody]Clients_DTO data)
        {
            return _objinter.editClientdata(data);
        }

        [Route("OnChangeTab2Inst")]
        public Clients_DTO OnChangeTab2Inst([FromBody]Clients_DTO data)
        {
            return _objinter.OnChangeTab2Inst(data);
        }

        [Route("saveClientMappingdata")]
        public Clients_DTO saveClientMappingdata([FromBody]Clients_DTO data)
        {
            return _objinter.saveClientMappingdata(data);
        }

        [Route("editClientMappingdata")]
        public Clients_DTO editClientMappingdata([FromBody]Clients_DTO data)
        {
            return _objinter.editClientMappingdata(data);
        }

        [Route("deactiveClientMappingdata")]
        public Clients_DTO deactiveClientMappingdata([FromBody]Clients_DTO data)
        {
            return _objinter.deactiveClientMappingdata(data);
        }

        [Route("modalListdata")]
        public Clients_DTO modalListdata([FromBody]Clients_DTO data)
        {
            return _objinter.modalListdata(data);
        }

        //VMS Client And IVRM Client Mapping 
        [Route("OnChangeClientTab3")]
        public Clients_DTO OnChangeClientTab3([FromBody]Clients_DTO data)
        {
            return _objinter.OnChangeClientTab3(data);
        }

        [Route("SaveVMSIVRMMapping")]
        public Clients_DTO SaveVMSIVRMMapping([FromBody]Clients_DTO data)
        {
            return _objinter.SaveVMSIVRMMapping(data);
        }
    }
}
