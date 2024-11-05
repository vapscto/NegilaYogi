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
    public class CovidVaccinationFacadeController : Controller
    {
        public CovidVaccinationInterface _interface;

        public CovidVaccinationFacadeController(CovidVaccinationInterface _inter)
        {
            _interface = _inter;
        }
        [Route("onloaddata")]
        public CovidVaccineDTO onloaddata([FromBody] CovidVaccineDTO data)
        {
            return _interface.onloaddata(data);
        }

        [Route("saverecord")]
        public CovidVaccineDTO saverecord([FromBody] CovidVaccineDTO data)
        {
            return _interface.saverecord(data);
        }
        [Route("deactiveY")]
        public CovidVaccineDTO deactiveY([FromBody] CovidVaccineDTO data)
        {
            return _interface.deactiveY(data);
        }

    }
}
