using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using PreadmissionDTOs.com.vaps.admission;
using PreadmissionDTOs.com.vaps.Transport;
using TransportServiceHub.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TransportServiceHub.Controllers
{
    [Route("api/[controller]")]
    public class StdRouteLocationMapFacade : Controller
    {
        public StdRouteLocationMapInterface _feegrouppagee;
        // GET: api/values
        public StdRouteLocationMapFacade(StdRouteLocationMapInterface maspag)
        {
            _feegrouppagee = maspag;
        }


        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPost]

        [Route("Getreportdetails")]
        public StdRouteLocationMapDTO Getreportdetails([FromBody]StdRouteLocationMapDTO data)
        {
            return _feegrouppagee.Getreportdetails(data);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        [Route("getdata")]
        public StdRouteLocationMapDTO getdata([FromBody] StdRouteLocationMapDTO data)
        {
            return _feegrouppagee.getdata(data);
        }

        [Route("get_sections")]
        public StdRouteLocationMapDTO get_sections([FromBody] StdRouteLocationMapDTO data)
        {
            return _feegrouppagee.get_sections(data);
        }
        [Route("get_cls_secs")]
        public StdRouteLocationMapDTO get_cls_secs([FromBody] StdRouteLocationMapDTO data)
        {
            return _feegrouppagee.get_cls_secs(data);
        }
        [Route("check_feegroup")]
        public StdRouteLocationMapDTO check_feegroup([FromBody] StdRouteLocationMapDTO data)
        {
            return _feegrouppagee.check_feegroup(data);
        }

        
        [Route("savedata")]
        public StdRouteLocationMapDTO savedata([FromBody] StdRouteLocationMapDTO data)
        {
            return _feegrouppagee.savedata(data);
        }

        
        [Route("getreport")]
        public StdRouteLocationMapDTO getreport([FromBody] StdRouteLocationMapDTO data)
        {
            return _feegrouppagee.getreport(data);
        }
        [Route("on_pic_route_change")]
        public StdRouteLocationMapDTO on_pic_route_change([FromBody] StdRouteLocationMapDTO data)
        {
            return _feegrouppagee.on_pic_route_change(data);
        }
        [Route("getreportedit")]
        public StdRouteLocationMapDTO getreportedit([FromBody] StdRouteLocationMapDTO data)
        {
            return _feegrouppagee.getreportedit(data);
        }
        [Route("deactivate")]
        public StdRouteLocationMapDTO deactivate([FromBody] StdRouteLocationMapDTO data)
        {
            return _feegrouppagee.deactivate(data);
        }


        

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [Route("get_data")]
        public StdRouteLocationMapDTO get_data([FromBody] StdRouteLocationMapDTO data)
        {
            return _feegrouppagee.get_data(data);
        }
    }
}
