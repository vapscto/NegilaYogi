using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Interfaces;
using PreadmissionDTOs;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class GenConfigFacadeController : Controller
    {
        public GenConfigInterface _IstudentmasterConfig;
        public GenConfigFacadeController(GenConfigInterface Iobj)
        {
            _IstudentmasterConfig = Iobj;
        }
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [Route("Configurationget")]
        public GeneralConfigDTO Configurationget([FromBody] GeneralConfigDTO data)
        {
            return _IstudentmasterConfig.Configurationget(data);
        }
        [Route("geteditdata")]
        public GeneralConfigDTO geteditdata([FromBody] GeneralConfigDTO data)
        {
            return _IstudentmasterConfig.geteditdata(data);
        }
        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        [HttpPost]
        [Route("savegenConfigData")]
        public GeneralConfigDTO saveMasterConfigData([FromBody] GeneralConfigDTO mstConfigData)
        {
            return _IstudentmasterConfig.saveMasterConfig(mstConfigData);
        }
        [HttpPost]
        [Route("getcontent")]
        public GeneralConfigDTO getcontent([FromBody] GeneralConfigDTO mstConfigData)
        {
            return _IstudentmasterConfig.getcontent(mstConfigData);
        }
        [Route("deleteUserNameconfig")]
        public GeneralConfigDTO deleteUserNameconfig([FromBody]GeneralConfigDTO id)
        {
            return _IstudentmasterConfig.deleteUserNameconfig(id);
        }


    }
}
