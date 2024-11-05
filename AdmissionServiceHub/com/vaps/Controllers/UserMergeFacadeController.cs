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
    public class UserMergeFacadeController : Controller
    {
        public UserMergeInterface _interface;

        public UserMergeFacadeController(UserMergeInterface _inter)
        {
            _interface = _inter;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("getalldetails")]
        public UserMergeDTO getalldetails([FromBody] UserMergeDTO data)
        {
            return _interface.getalldetails(data);
        }

        [Route("onstudentnamechange")]
        public UserMergeDTO onstudentnamechange([FromBody] UserMergeDTO data)
        {
            return _interface.onstudentnamechange(data);
        }

        [Route("mergeuserdetails")]
        public UserMergeDTO mergeuserdetails([FromBody] UserMergeDTO data)
        {
            return _interface.mergeuserdetails(data);
        }
    }
}
