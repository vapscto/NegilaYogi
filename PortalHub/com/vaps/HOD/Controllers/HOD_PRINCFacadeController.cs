using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.FrontOffice;
using PreadmissionDTOs.com.vaps.Portals.HOD;
using PortalHub.com.vaps.HOD.Interfaces;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalHub.com.vaps.HOD.Controllers
{
    [Route("api/[controller]")]
    public class HOD_PRINCFacadeController : Controller
    {
        HOD_PRINCInterface _org;

        public HOD_PRINCFacadeController(HOD_PRINCInterface org1)
        {
            _org = org1;
        }

        [Route("getalldetails")]
        public HOD_DTO getalldetails([FromBody] HOD_DTO data)
        {
            return _org.getdata(data);
        }

        [Route("save")]
        public HOD_DTO savedata([FromBody] HOD_DTO data)
        {
            return _org.savedata(data);
        }

        [Route("mappHOD")]
        public HOD_DTO mappHOD([FromBody] HOD_DTO data)
        {
            return _org.mappHODdata(data);
        }

        [Route("updateHOD")]
        public HOD_DTO updateHOD([FromBody] HOD_DTO data)
        {
            return _org.updateHOD(data);
        }
        [Route("deactiveY")]
        public HOD_DTO deactiveY([FromBody] HOD_DTO data)
        {
            return _org.deactiveY(data);
        }

    }
}
