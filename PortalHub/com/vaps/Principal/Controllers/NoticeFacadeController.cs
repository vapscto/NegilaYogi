using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.Principal;
using PortalHub.com.vaps.Principal.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalHub.com.vaps.Principal.Controllers
{
    [Route("api/[controller]")]
    public class NoticeFacadeController : Controller
    {
        public NoticeInterface _notic;
        public NoticeFacadeController(NoticeInterface notic)
        {
            _notic = notic;
        }
        // GET: api/values
        [Route("savedetail")]
        public Notice_DTO savedetail([FromBody]Notice_DTO data)
        {
            return _notic.savedetail(data);
        }
        [Route("Getdetails")]
        public Notice_DTO Getdetails([FromBody]Notice_DTO data)
        {
            return _notic.Getdetails(data);
        }
        [Route("deactivate")]
        public Notice_DTO deactivate([FromBody]Notice_DTO data)
        {
            return _notic.deactivate(data);
        }
    }
}
