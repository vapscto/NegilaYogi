using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PortalHub.com.vaps.Employee.Interfaces;
using PreadmissionDTOs.com.vaps.IssueManager.PettyCash;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using PreadmissionDTOs.com.vaps.VMS.Training;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace PortalHub.com.vaps.Employee.Controllers
{
    [Route("api/[controller]")]
    public class CovidTestUploadFacadeController : Controller
    {
        public CovidTestUploadInterface _interface;

        public CovidTestUploadFacadeController(CovidTestUploadInterface _inter)
        {
            _interface = _inter;
        }
        [Route("onloaddata")]
        public CovidTestUploadDTO onloaddata([FromBody] CovidTestUploadDTO data)
        {
            return _interface.onloaddata(data);
        }

        [Route("saverecord")]
        public CovidTestUploadDTO saverecord([FromBody] CovidTestUploadDTO data)
        {
            return _interface.saverecord(data);
        }

        [Route("deactiveY")]
        public CovidTestUploadDTO deactiveY([FromBody] CovidTestUploadDTO data)
        {
            return _interface.deactiveY(data);
        }
    }
}
