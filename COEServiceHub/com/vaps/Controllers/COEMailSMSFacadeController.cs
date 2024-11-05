using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoeServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.COE;

namespace CoeServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class COEMailSMSFacadeController : Controller
    {
        public COEMailSMSInterface _inter;

        public COEMailSMSFacadeController(COEMailSMSInterface maspag)
        {
            _inter = maspag;
        }
        [Route("getdata/{id:int}")]
        public void getdata(int id)
        {
            _inter.getdata(id);
        }
    }
}
