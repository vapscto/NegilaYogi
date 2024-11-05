using LibraryServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class OpacSearchFacade:Controller
    {
        public OpacSearchInterface _org;

        public OpacSearchFacade(OpacSearchInterface orga)
        {
            _org = orga;
        }
        [Route("getalldetails")]
        public OpacSearchDTO getalldetails([FromBody] OpacSearchDTO data)
        {
            return _org.getalldetails(data);
        }
        [Route("report")]
        public Task<OpacSearchDTO> report([FromBody] OpacSearchDTO data)
        {
            return _org.report(data);
        }
    }
}
