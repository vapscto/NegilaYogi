using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.PettyCash.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager.PettyCash;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IssueManager.com.PettyCash.Facade
{
    [Route("api/[controller]")]
    public class PC_RequisitionFacadeController : Controller
    {
        public PC_RequisitionInterface _interface;
        public PC_RequisitionFacadeController(PC_RequisitionInterface _inter)
        {
            _interface = _inter;
        }


        [Route("onloaddata")]
        public PC_RequisitionDTO onloaddata([FromBody] PC_RequisitionDTO data)
        {
            return _interface.onloaddata(data);
        }

        [Route("onchangedept")]
        public PC_RequisitionDTO onchangedept([FromBody] PC_RequisitionDTO data)
        {
            return _interface.onchangedept(data);
        }

        [Route("saverecord")]
        public PC_RequisitionDTO saverecord([FromBody] PC_RequisitionDTO data)
        {
            return _interface.saverecord(data);
        }

        [Route("EditData")]
        public PC_RequisitionDTO EditData([FromBody] PC_RequisitionDTO data)
        {
            return _interface.EditData(data);
        }

        [Route("deactiveY")]
        public PC_RequisitionDTO deactiveY([FromBody] PC_RequisitionDTO data)
        {
            return _interface.deactiveY(data);
        }

        [Route("Viewdata")]
        public PC_RequisitionDTO Viewdata([FromBody] PC_RequisitionDTO data)
        {
            return _interface.Viewdata(data);
        }

        [Route("deactiveparticulars")]
        public PC_RequisitionDTO deactiveparticulars([FromBody] PC_RequisitionDTO data)
        {
            return _interface.deactiveparticulars(data);
        }
    }
}
