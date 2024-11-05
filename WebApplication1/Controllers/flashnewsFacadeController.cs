using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Interfaces;
using WebApplication1.Services;
using PreadmissionDTOs;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class flashnewsFacadeController : Controller
    {
        public flashnewsInterface _enq;

        public flashnewsFacadeController(flashnewsInterface enqui)
        {
            _enq = enqui;
        }

        // GET api/values/5
       
        [HttpPost]
        public async Task<regis> Post([FromBody] regis enquiry)
        {
            return await _enq.saveEnqdata(enquiry);
        }

       
    }
}
