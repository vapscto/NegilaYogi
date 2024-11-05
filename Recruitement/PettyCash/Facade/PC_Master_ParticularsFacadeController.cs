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
    public class PC_Master_ParticularsFacadeController : Controller
    {
        public PC_Master_ParticularsInterface _interface;

        public PC_Master_ParticularsFacadeController(PC_Master_ParticularsInterface _inter)
        {
            _interface = _inter;
        }
        [Route("onloaddata")]
        public PC_Master_ParticularsDTO onloaddata([FromBody] PC_Master_ParticularsDTO data)
        {
            return _interface.onloaddata(data);
        }

        [Route("saverecord")]
        public PC_Master_ParticularsDTO saverecord([FromBody] PC_Master_ParticularsDTO data)
        {
            return _interface.saverecord(data);
        }

        [Route("deactiveY")]
        public PC_Master_ParticularsDTO deactiveY([FromBody] PC_Master_ParticularsDTO data)
        {
            return _interface.deactiveY(data);
        }
    }
}
