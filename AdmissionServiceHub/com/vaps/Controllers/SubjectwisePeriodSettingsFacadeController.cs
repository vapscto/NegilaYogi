using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdmissionServiceHub.com.vaps.Interfaces;
using AdmissionServiceHub.com.vaps.Services;
using PreadmissionDTOs;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AdmissionServiceHub.com.vapstech.Controllers
{
    [Route("api/[controller]")]

    public class SubjectwisePeriodSettingsFacadeController : Controller
    {
        public SubjectwisePeriodSettingsInterface _SubjectwisePeriodSettings;


        public SubjectwisePeriodSettingsFacadeController(SubjectwisePeriodSettingsInterface SubjectwisePeriodSettings)
        {
            _SubjectwisePeriodSettings = SubjectwisePeriodSettings;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
       // [HttpGet]

        [Route("Getdetails")]
        public SubjectwisePeriodSettingsDTO Getdetails([FromBody] SubjectwisePeriodSettingsDTO dto)//int IVRMM_Id
        {
            return _SubjectwisePeriodSettings.GetData(dto);
        }


        [HttpPost]
        public SubjectwisePeriodSettingsDTO Post([FromBody] SubjectwisePeriodSettingsDTO masterMDT)
        {
            // return _reg.getregdata(reg);
            return _SubjectwisePeriodSettings.SaveData(masterMDT);
        }
        [Route("getsubjectMaxPeriod")]
        
              public SubjectwisePeriodSettingsDTO subjectMaxPeriod([FromBody] SubjectwisePeriodSettingsDTO masterMDT)
        {
            // return _reg.getregdata(reg);
            return _SubjectwisePeriodSettings.subjectMaxPeriod(masterMDT);
        }



    }
}
