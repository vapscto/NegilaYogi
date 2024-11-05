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

    public class WrittenTestMarksEntryFacadeController : Controller
    {
        public WrittenTestMarksEntryInterface _MasterModule;


        public WrittenTestMarksEntryFacadeController(WrittenTestMarksEntryInterface MasterModule)
        {
            _MasterModule = MasterModule;
        }

        // GET api/values/5

       // [HttpGet]
   
       

        // POST api/values
        [HttpPost]
        public WirttenTestSubjectWiseMarksEntryDTO Post([FromBody] WirttenTestSubjectWiseMarksEntryDTO masterMDT)
        {
            // return _reg.getregdata(reg);
            return _MasterModule.WrittenTestMarksEntryData(masterMDT);
        }

        [Route("GetWrittenTestMarks/")]
        public WrittenTestMarksBindDataDTO GetWrittenTestMarks([FromBody] WrittenTestMarksBindDataDTO masterMDT)
        {
            // return _reg.getregdata(reg);
            var fsdf= _MasterModule.GetWrittenTestMarks(masterMDT);
            return _MasterModule.GetWrittenTestMarks(masterMDT);
        }

        [Route("GetdetailsOnSchedule/")]

        public WirttenTestSubjectWiseMarksEntryDTO GetdetailsOnSchedule([FromBody] WirttenTestSubjectWiseMarksEntryDTO masterMDT)//int IVRMM_Id
        {
            return _MasterModule.GetdetailsOnSchedule(masterMDT);
        }

        [Route("Getdetails/")]

        public WrittenTestMarksBindDataDTO Getdetails([FromBody] WrittenTestMarksBindDataDTO WrittenTestMarksBindDataDTO)//int IVRMM_Id
        {
            return _MasterModule.GetWrittenTestMarksEntryData(WrittenTestMarksBindDataDTO);
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
