using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoeServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.COE;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CoeServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class COEFacade : Controller
    {
        COEInterface _intterface;
        public COEFacade(COEInterface intterface)
        {
            _intterface = intterface;
        }
        [Route("getdata")]
        public COEDTO getdata([FromBody] COEDTO obj)
        {
            return _intterface.getdata(obj);
        }
        [Route("getEvents")]
        public COEDTO getEvents([FromBody] COEDTO obj)
        {
            return _intterface.getEvents(obj);
        }
        [Route("sendmsg")]
        public COEDTO sendmsg([FromBody] COEDTO data)
        {
            return _intterface.sendmsg(data);
        }


    }
}
