
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PortalHub.com.vaps.Chairman.Interfaces;
using PreadmissionDTOs.com.vaps.Portals.Chirman;
//using AdmissionServiceHub.com.vaps.Interfaces;


using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalHub.com.vaps.Chairman.Controllers
{
    [Route("api/[controller]")]
    public class FEESGroupHeadWiseDetailsSchoolFacadeController : Controller
    {
        public FEESGroupHeadWiseDetailsSchoolInterface _ChairmanDashboardReport;

        public FEESGroupHeadWiseDetailsSchoolFacadeController(FEESGroupHeadWiseDetailsSchoolInterface data)
        {
            _ChairmanDashboardReport = data;
        }


        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPost]
        [Route("Getdetails")]
        public FEESGroupHeadWiseDetailsSchoolDTO Getdetails([FromBody] FEESGroupHeadWiseDetailsSchoolDTO data)//int IVRMM_Id
        {
            return  _ChairmanDashboardReport.Getdetails(data);
        }

        
          [HttpPost]
        [Route("Getsectioncount")]
        public FEESGroupHeadWiseDetailsSchoolDTO Getsectioncount([FromBody] FEESGroupHeadWiseDetailsSchoolDTO data)
        {
            return _ChairmanDashboardReport.Getsectioncount(data);
        }

        [HttpPost]
        [Route("Getgroupclasscount")]
        public FEESGroupHeadWiseDetailsSchoolDTO Getgroupclasscount([FromBody] FEESGroupHeadWiseDetailsSchoolDTO data)
        {
            return _ChairmanDashboardReport.Getgroupclasscount(data);
        }
        

    }
}
