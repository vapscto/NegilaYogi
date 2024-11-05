using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VisitorsManagement;
using VisitorsManagementServiceHub.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VisitorsManagementServiceHub.Controllers
{
    [Route("api/[controller]")]
    public class InwardFacadeController : Controller
    {
        InwardInterface interobj;
        public InwardFacadeController(InwardInterface obj)
        {
            interobj = obj;
        }

      
        // POST api/<controller>
        [HttpPost]
        [Route("getDetails")]
        public InwardDTO getDetails([FromBody]InwardDTO data)
        {
            return interobj.getDetails(data);
        }

        [Route("EditDetails")]
        public InwardDTO EditDetails([FromBody] InwardDTO id)
        {
            return interobj.EditDetails(id);
        }

        [Route("saveData")]
        public InwardDTO saveData([FromBody]InwardDTO data)
        {
            return interobj.saveData(data);
        }

        [Route("deactivate")]
        public InwardDTO deactivate([FromBody]InwardDTO data)
        {
            return interobj.deactivate(data);
        }

        [Route("searchfilter")]
        public InwardDTO searchfilter([FromBody]InwardDTO value)
        {
            return interobj.searchfilter(value);
        }

        [Route("get_empdetails")]
        public InwardDTO get_empdetails([FromBody]InwardDTO value)
        {
            return interobj.get_empdetails(value);
        }

        [Route("searchfilter2")]
        public InwardDTO searchfilter2([FromBody]InwardDTO value)
        {
            return interobj.searchfilter2(value);
        }

        [Route("get_empdetails2")]
        public InwardDTO get_empdetails2([FromBody]InwardDTO value)
        {
            return interobj.get_empdetails2(value);
        }
        
    }
}
