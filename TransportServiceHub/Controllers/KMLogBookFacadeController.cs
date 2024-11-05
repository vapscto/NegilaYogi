using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TransportServiceHub.Interfaces;
using PreadmissionDTOs.com.vaps.Transport;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TransportServiceHub.Controllers
{
    [Route("api/[controller]")]
    public class KMLogBookFacadeController : Controller
    {
        public KMLogBookInterface driverint;

        public KMLogBookFacadeController(KMLogBookInterface driv)
        {
            driverint = driv;
        }
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [Route("getdata/{id:int}")]
        public KMLogBookDTO getdata(int id)
        {
            return driverint.getdata(id);
        }
        [Route("getreportdata/{id:int}")]
        public KMLogBookDTO getreportdata(int id)
        {
            return driverint.getreportdata(id);
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
        [Route("savedata")]
        public KMLogBookDTO savedata([FromBody] KMLogBookDTO data)
        {
            return driverint.savedata(data);
        }
        [Route("getkmreport")]
        public KMLogBookDTO getkmreport([FromBody] KMLogBookDTO data)
        {
            return driverint.getkmreport(data);
        }
        [Route("Onvahiclechange")]
        public KMLogBookDTO Onvahiclechange([FromBody] KMLogBookDTO data)
        {
            return driverint.Onvahiclechange(data);
        }
        [Route("vehicletypechange")]
        public KMLogBookDTO vehicletypechange([FromBody] KMLogBookDTO data)
        {
            return driverint.vehicletypechange(data);
        }
        [Route("deleterecord")]
        public KMLogBookDTO deleterecord([FromBody] KMLogBookDTO data)
        {
            return driverint.deleterecord(data);
        }
        
        [Route("edit")]
        public KMLogBookDTO edit([FromBody] KMLogBookDTO data)
        {
            return driverint.edit(data);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
