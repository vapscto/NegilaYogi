using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PortalHub.com.vaps.Employee.Interfaces;
using PreadmissionDTOs.com.vaps.Portals.Employee;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalHub.com.vaps.Employee.Controllers
{
    [Route("api/[controller]")]
    public class HWMonthEndReportFacade : Controller
    {
        public HWMonthEndReportInterface _hwtar;

        public HWMonthEndReportFacade(HWMonthEndReportInterface maspag)
        {
            _hwtar = maspag;
        }

        [HttpPost]
        [Route("getalldetails123")]
        public HomeWorkUploadDTO getalldetails123([FromBody] HomeWorkUploadDTO data)
        {
            return _hwtar.getdata123(data);
        }

        [Route("getreport")]
        public Task<HomeWorkUploadDTO> getreport([FromBody] HomeWorkUploadDTO data)
        {
            return _hwtar.getreport(data);
        }

        
    }
}
