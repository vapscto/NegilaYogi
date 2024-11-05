using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.Chirman;
using PortalHub.com.vaps.HOD.Interfaces;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalHub.com.vaps.HOD.Controllers
{
    [Route("api/[controller]")]
    public class HODFeesCollectionFacadeController : Controller
    {
        public HODFeesCollectionInterface _ChairmanDashboardReport;

        public HODFeesCollectionFacadeController(HODFeesCollectionInterface data)
        {
            _ChairmanDashboardReport = data;
        }
        // GET: /<controller>/
      
        [HttpPost]
        [Route("Getdetails")]
        public FEESOverAllStatusSchoolDTO Getdetails([FromBody] FEESOverAllStatusSchoolDTO data)//int IVRMM_Id
        {
            return _ChairmanDashboardReport.Getdetails(data);
        }

        [Route("Getsectioncount")]
        public FEESOverAllStatusSchoolDTO Getsectioncount([FromBody] FEESOverAllStatusSchoolDTO data)
        {
            return _ChairmanDashboardReport.Getsectioncount(data);
        }
    }
}
