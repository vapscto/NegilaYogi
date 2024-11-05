using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.PettyCash.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager.PettyCash;
using PreadmissionDTOs.com.vaps.VMS.Training;
using Recruitment.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Recruitment.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class TrainingtypewisereportFacadeController : Controller
    {
        public TrainingtypewisereportInterface _interface;

        public TrainingtypewisereportFacadeController(TrainingtypewisereportInterface _inter)
        {
            _interface = _inter;
        }
        [Route("onloaddata")]
        public TrainingtypewisereportDTO onloaddata([FromBody] TrainingtypewisereportDTO data)
        {
            return _interface.onloaddata(data);
        }

        [Route("getreport")]
        public TrainingtypewisereportDTO getreport([FromBody] TrainingtypewisereportDTO data)
        {
            return _interface.getreport(data);
        }


    }
}
