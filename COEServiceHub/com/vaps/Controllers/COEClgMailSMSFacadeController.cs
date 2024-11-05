using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using COEServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace COEServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class COEClgMailSMSFacadeController : Controller
    {
        public COEClgMailSMSInterface _inter;

        public COEClgMailSMSFacadeController(COEClgMailSMSInterface maspag)
        {
            _inter = maspag;
        }

        [Route("clg_getdetail/{id:int}")]
        public void clg_getdetail(int id)
        {
            _inter.clg_getdetail(id);
        }
    }
}
