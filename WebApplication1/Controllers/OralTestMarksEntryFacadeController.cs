using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Interfaces;
using WebApplication1.Services;
using PreadmissionDTOs;
using Microsoft.AspNetCore.Http;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]

    public class OralTestMarksEntryFacadeController : Controller
    {
        public OralTestMarksEntryInterface _MasterModule;


        public OralTestMarksEntryFacadeController(OralTestMarksEntryInterface MasterModule)
        {
            _MasterModule = MasterModule;
        }

        // GET api/values/5

      
        [Route("Getdetails/")]

        public OralTestMarksBindDataDTO Getdetails([FromBody]OralTestOralByMarksDTO masterMDT)//int IVRMM_Id
        {
            return _MasterModule.GetOralTestMarksEntryData(masterMDT);
        }

   


        // POST api/values
        [HttpPost]
        public OralTestOralByMarksDTO Post([FromBody] OralTestOralByMarksDTO masterMDT)
        {
            // return _reg.getregdata(reg);
            return _MasterModule.OralTestMarksEntryData(masterMDT);
        }

        [Route("GetOralTestMarks/")]
        public OralTestMarksBindDataDTO[] GetOralTestMarks([FromBody] OralTestMarksBindDataDTO masterMDT)
        {
            // return _reg.getregdata(reg);
            return _MasterModule.GetOralTestMarks(masterMDT);
        }

        [Route("GetdetailsOnSchedule/")]

        public OralTestOralByMarksDTO GetdetailsOnSchedule([FromBody] OralTestOralByMarksDTO masterMDT)//int IVRMM_Id
        {
            return _MasterModule.GetdetailsOnSchedule(masterMDT);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {

        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {

        }
    }
}
