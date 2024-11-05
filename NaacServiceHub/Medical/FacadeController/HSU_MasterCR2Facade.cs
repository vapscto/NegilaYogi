using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NaacServiceHub.Medical.Interface;
using PreadmissionDTOs.NAAC.Medical;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.Medical.FacadeController
{
    [Route("api/[controller]")]
    public class HSU_MasterCR2Facade : Controller
    {
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        public HSU_MasterCR2Interface _Interface;
        public HSU_MasterCR2Facade(HSU_MasterCR2Interface para1) {
            _Interface = para1;
        }

        [Route("loaddata")]
        public HSU_MasterCR2_DTO loaddata([FromBody] HSU_MasterCR2_DTO data)
        {
            return _Interface.loaddata(data);
        }
        [Route("save_HSU_221")]
        public HSU_MasterCR2_DTO save_HSU_221([FromBody] HSU_MasterCR2_DTO data)
        {
            return _Interface.save_HSU_221(data);
        }
        [Route("save_HSU_232")]
        public HSU_MasterCR2_DTO save_HSU_232([FromBody]HSU_MasterCR2_DTO data)
        {
            return _Interface.save_HSU_232(data);
        }
        [Route("save_HSU_255")]
        public HSU_MasterCR2_DTO save_HSU_255([FromBody] HSU_MasterCR2_DTO data)
        {
            return _Interface.save_HSU_255(data);
        }
    }
}
