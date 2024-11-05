using FeeServiceHub.com.vaps.interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.controllers
{
    
    
        [Route("api/[controller]")]
        public class Financial_YearFacade : Controller
        {
            public Financial_YearInterface _org;

            public Financial_YearFacade (Financial_YearInterface orga)
            {
                _org = orga;
            }

            [HttpPost]
            [Route("loaddata")]
            public Financial_YearDTO loaddata([FromBody] Financial_YearDTO data)
            {
                return _org.loaddata(data);
            }
            [Route("save")]
            public Financial_YearDTO save([FromBody] Financial_YearDTO data)
            {
                return _org.save(data);
            }
            
        }
    
}
