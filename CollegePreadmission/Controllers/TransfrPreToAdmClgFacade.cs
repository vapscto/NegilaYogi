using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegePreadmission.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Preadmission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegePreadmission.Controllers
{
    [Route("api/[controller]")]
    public class TransfrPreToAdmClgFacade : Controller
    {
        public TransfrPreToAdmClgInterface _ads;
        public TransfrPreToAdmClgFacade(TransfrPreToAdmClgInterface interf)
        {
            _ads = interf;
        }

        [Route("onloadgetdetails")]
        public TransfrPreToAdmDTO onloadgetdetails([FromBody]TransfrPreToAdmDTO dto)
        {
            return _ads.onloadgetdetails(dto);
        }
        [Route("get_branchs")]
        public TransfrPreToAdmDTO get_branchs([FromBody] TransfrPreToAdmDTO dt)
        {
            return _ads.get_branchs(dt);
        }

        [Route("get_semester")]
        public TransfrPreToAdmDTO get_semester([FromBody] TransfrPreToAdmDTO dt)
        {
            return _ads.get_semester(dt);
        }
        [Route("getserdata")]
        public TransfrPreToAdmDTO getserdata([FromBody] TransfrPreToAdmDTO dt)
        {
            return _ads.getserdata(dt);
        }

        [Route("expoadmi")]
        public async Task<TransfrPreToAdmDTO> expoadmi([FromBody] TransfrPreToAdmDTO data)
        {
            return await _ads.expoadmi(data);
        }
    }
}
