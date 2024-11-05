using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using WebApplication1.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class InstitutionUserMappingFacadeController : Controller
    {
        public InstitutionUserMappingInterface _interface;
        public InstitutionUserMappingFacadeController(InstitutionUserMappingInterface inter)
        {
            _interface = inter;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("loaddata")]
        public InstitutionUserMappingDTO loaddata([FromBody] InstitutionUserMappingDTO data)
        {
            return _interface.loaddata(data);
        }

        [Route("onchangeinst")]
        public InstitutionUserMappingDTO onchangeinst([FromBody] InstitutionUserMappingDTO data)
        {
            return _interface.onchangeinst(data);
        }

        [Route("savedetails")]
        public InstitutionUserMappingDTO savedetails([FromBody] InstitutionUserMappingDTO data)
        {
            return _interface.savedetails(data);
        }

        [Route("viewdetails")]
        public InstitutionUserMappingDTO viewdetails([FromBody] InstitutionUserMappingDTO data)
        {
            return _interface.viewdetails(data);
        }


        // VMS RELATED APIS
        [Route("savepaymentremarks")]
        public InstitutionUserMappingDTO savepaymentremarks([FromBody] InstitutionUserMappingDTO data)
        {
            return _interface.savepaymentremarks(data);
        }

        [Route("getvmspaymentsubsctiptionreport")]
        public InstitutionUserMappingDTO getvmspaymentsubsctiptionreport([FromBody] InstitutionUserMappingDTO data)
        {
            return _interface.getvmspaymentsubsctiptionreport(data);
        }
    }
}