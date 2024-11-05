using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class SMSController : Controller
    {
        readonly ILogger<SMSController> _logger;
        public SMSController(ILogger<SMSController> log)
        {
            _logger = log;
        }

        [HttpPost]
        [Route("getsmsparameters")]
        public SMSParameters GetInstituteDetailsByparam([FromBody] SMSParameters data)
        {
            _logger.LogError(data.messagepart);
            _logger.LogError(data.mobilenumber.ToString());
            _logger.LogError(data.status.ToString());
            _logger.LogError(data.Donetime.ToString());

            _logger.LogInformation(data.messagepart);
            _logger.LogInformation(data.mobilenumber.ToString());
            _logger.LogInformation(data.status.ToString());
            _logger.LogInformation(data.Donetime.ToString());

            _logger.LogDebug(data.messagepart);
            _logger.LogDebug(data.mobilenumber.ToString());
            _logger.LogDebug(data.status.ToString());
            _logger.LogDebug(data.Donetime.ToString());

            return data;
        }

        [HttpGet]
        [Route("getsmsparametersget")]
        public SMSParameters GetInstituteDetailsById(SMSParameters data)
        {
            _logger.LogError(data.messagepart);
            _logger.LogError(data.mobilenumber.ToString());
            _logger.LogError(data.status.ToString());
            _logger.LogError(data.Donetime.ToString());

            _logger.LogInformation(data.messagepart);
            _logger.LogInformation(data.mobilenumber.ToString());
            _logger.LogInformation(data.status.ToString());
            _logger.LogInformation(data.Donetime.ToString());

            _logger.LogDebug(data.messagepart);
            _logger.LogDebug(data.mobilenumber.ToString());
            _logger.LogDebug(data.status.ToString());
            _logger.LogDebug(data.Donetime.ToString());

            return data;
        }
    }
}

