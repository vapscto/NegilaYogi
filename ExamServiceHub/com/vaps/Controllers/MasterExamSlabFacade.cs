
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using PreadmissionDTOs.com.vaps.COE;
using PreadmissionDTOs.com.vaps.Exam;
using ExamServiceHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class MasterExamSlabFacade : Controller
    {
        public MasterExamSlabInterface _ttcategory;

        public MasterExamSlabFacade(MasterExamSlabInterface maspag)
        {
            _ttcategory = maspag;
        } 
        [HttpGet]
        [Route("getdetails/{id:int}")]
        public MasterExamSlabDTO getorgdet(int id)
        {
            return _ttcategory.getdetails(id);
        }
       
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        [HttpPost]
        [Route("savedetail")]
        public MasterExamSlabDTO Post([FromBody] MasterExamSlabDTO org)
        {
            return _ttcategory.savedetail(org);
        }
       
    
    }
}
