
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
    public class ChairmanloginCountFacadeController : Controller
    {
        public ChairmanloginCountInterface _ChairmanDashboardReport;

        public ChairmanloginCountFacadeController(ChairmanloginCountInterface data)
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
        public ChairmanloginCountDTO Getdetails([FromBody] ChairmanloginCountDTO data)//int IVRMM_Id
        {
            return  _ChairmanDashboardReport.Getdetails(data);
        }             
        [HttpPost]
        [Route("getstdappcount")]
        public ChairmanloginCountDTO getstdappcount([FromBody] ChairmanloginCountDTO data)
        {
            return _ChairmanDashboardReport.getstdappcount(data);
        }
        [HttpPost]
        [Route("getstaffappcount")]
        public ChairmanloginCountDTO getstaffappcount([FromBody] ChairmanloginCountDTO data)
        {
            return _ChairmanDashboardReport.getstaffappcount(data);
        }
        [HttpPost]
        [Route("GetpopupDetails")]
        public ChairmanloginCountDTO GetpopupDetails([FromBody] ChairmanloginCountDTO data)
        {
            return _ChairmanDashboardReport.GetpopupDetails(data);
        }

        
    }
}
