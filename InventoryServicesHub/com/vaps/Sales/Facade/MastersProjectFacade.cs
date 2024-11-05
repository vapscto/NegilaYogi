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
    public class MastersProjectFacade : Controller
    {
        public MastersProjectInterface _objinter;

        public MastersProjectFacade(MastersProjectInterface parameter)
        {
            _objinter = parameter;
        }

        [Route("getdetails")]
        public MastersProject_DTO getdetails([FromBody] MastersProject_DTO dTO)
        {
            return _objinter.getdetails(dTO);
        }

        [Route("OnChangeInstitution")]
        public MastersProject_DTO OnChangeInstitution([FromBody]MastersProject_DTO value)
        {
            return _objinter.OnChangeInstitution(value);
        }

        [Route("saverecord")]
        public MastersProject_DTO saverecord([FromBody]MastersProject_DTO value)
        {
            return _objinter.saverecord(value);
        }

        [Route("deactiveY")]
        public MastersProject_DTO deactiveY([FromBody]MastersProject_DTO value)
        {
            return _objinter.deactiveY(value);
        }
    }
}
