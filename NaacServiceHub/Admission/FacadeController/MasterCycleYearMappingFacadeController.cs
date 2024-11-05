using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NaacServiceHub.Admission.Interface;
using PreadmissionDTOs.NAAC.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.Admission.FacadeController
{
    [Route("api/[controller]")]
    public class MasterCycleYearMappingFacadeController : Controller
    {
        public MasterCycleYearMappingInterface _ads;

        public MasterCycleYearMappingFacadeController(MasterCycleYearMappingInterface adstu)
        {
            _ads = adstu;
        }
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }     
        
        [HttpPost("getalldetails")]
        public MasterCycleYearMappingDTO getalldetails([FromBody] MasterCycleYearMappingDTO data)
        {
            return _ads.getalldetails(data);
        }
        [HttpPost("savedetails")]
        public MasterCycleYearMappingDTO savedetails([FromBody] MasterCycleYearMappingDTO data)
        {
            return _ads.savedetails(data);
        }
        [HttpPost("activedeactivedetails")]
        public MasterCycleYearMappingDTO activedeactivedetails([FromBody] MasterCycleYearMappingDTO data)
        {
            return _ads.activedeactivedetails(data);
        }
        [HttpPost("editdetails")]
        public MasterCycleYearMappingDTO editdetails([FromBody] MasterCycleYearMappingDTO data)
        {
            return _ads.editdetails(data);
        }
        [HttpPost("getOrder")]
        public MasterCycleYearMappingDTO getOrder([FromBody] MasterCycleYearMappingDTO data)
        {
            return _ads.getOrder(data);
        }

        // Master  Cycle Year Mapping
        [HttpPost("onchangecycle")]
        public MasterCycleYearMappingDTO onchangecycle([FromBody] MasterCycleYearMappingDTO data)
        {
            return _ads.onchangecycle(data);
        }
        [HttpPost("savedetails1")]
        public MasterCycleYearMappingDTO savedetails1([FromBody] MasterCycleYearMappingDTO data)
        {
            return _ads.savedetails1(data);
        }
        [HttpPost("viewdetails")]
        public MasterCycleYearMappingDTO viewdetails([FromBody] MasterCycleYearMappingDTO data)
        {
            return _ads.viewdetails(data);
        }

        [HttpPost("deactivesem")]
        public MasterCycleYearMappingDTO deactivesem([FromBody] MasterCycleYearMappingDTO data)
        {
            return _ads.deactivesem(data);
        }
        [HttpPost("delete")]
        public MasterCycleYearMappingDTO delete([FromBody] MasterCycleYearMappingDTO data)
        {
            return _ads.delete(data);
        }

    }
}
