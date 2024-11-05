using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdmissionServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdmissionServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class AdhaarNotEnteredListFacadeController : Controller
    {
        public AdhaarNotEnteredListInterface _AttenRpt;

        public AdhaarNotEnteredListFacadeController(AdhaarNotEnteredListInterface AttenRpt)
        {
            _AttenRpt = AttenRpt;
        }
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        // load initial dropdown
        [Route("getInitailData")]
        public Task<AdhaarNotEnteredListDTO> getInitailData([FromBody]AdhaarNotEnteredListDTO mi_id)
        {
            return _AttenRpt.getInitailData(mi_id);
        }
        [Route("getsection")]
        public Task<AdhaarNotEnteredListDTO> getsection([FromBody]AdhaarNotEnteredListDTO mi_id)
        {
            return _AttenRpt.getsection(mi_id);
        }
        [Route("getclass")]
        public Task<AdhaarNotEnteredListDTO> getclass([FromBody]AdhaarNotEnteredListDTO mi_id)
        {
            return _AttenRpt.getclass(mi_id);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        [Route("searchdata")]
        public Task<AdhaarNotEnteredListDTO> searchdata([FromBody] AdhaarNotEnteredListDTO data)
        {
            return _AttenRpt.getserdata(data);
        }



        //classchamge

        [HttpPost]
        [Route("searchdataclass")]
        public Task<ClassChangeDTO> searchdataclass([FromBody] ClassChangeDTO data)
        {
            return _AttenRpt.searchdataclass(data);
        }

        [Route("getInitailyear")]
        public Task<ClassChangeDTO> getInitailyear([FromBody]ClassChangeDTO mi_id)
        {
            return _AttenRpt.getInitailyear(mi_id);
        }

        [HttpPost]
        [Route("getstudents")]
        public Task<AdhaarNotEnteredListDTO> getstudents([FromBody] AdhaarNotEnteredListDTO data)
        {
            return _AttenRpt.getstudents(data);
        }

        //sate and country wise student data
        [HttpPost]
        [Route("Getcountrystatedata")]
        public Task<AdhaarNotEnteredListDTO> Getcountrystatedata([FromBody] AdhaarNotEnteredListDTO data)
        {
            return _AttenRpt.Getcountrystatedata(data);
        }
        [Route("getEntryType")]
        public Task<AdhaarNotEnteredListDTO> getEntryType([FromBody] AdhaarNotEnteredListDTO data)
        {
            return _AttenRpt.getEntryType(data);
        }
        [Route("getsectionlist")]
        public Task<AdhaarNotEnteredListDTO> getsectionlist([FromBody] AdhaarNotEnteredListDTO data)
        {
            return _AttenRpt.getsectionlist(data);
        }
        [Route("getAttendencenotDoneReport")]
        public Task<AdhaarNotEnteredListDTO> getAttendencenotDoneReport([FromBody] AdhaarNotEnteredListDTO data)
        {
            return _AttenRpt.getAttendencenotDoneReport(data);
        }
        [Route("getClassEntryType")]
        public Task<AdhaarNotEnteredListDTO> getClassEntryType([FromBody] AdhaarNotEnteredListDTO data)
        {
            return _AttenRpt.getClassEntryType(data);
        }

        [Route("emailsend")]
        public AdhaarNotEnteredListDTO emailsend([FromBody]AdhaarNotEnteredListDTO data)
        {
            return _AttenRpt.emailsend(data);
        }
    }
}
