using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClgCOEServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ClgCOEServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class ClgCOEMailSMSFacade : Controller
    {

        public ClgCOEMailSMSInterface _inter;

        public ClgCOEMailSMSFacade(ClgCOEMailSMSInterface maspag)
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
