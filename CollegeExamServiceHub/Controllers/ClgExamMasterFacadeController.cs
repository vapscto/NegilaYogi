using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegeExamServiceHub.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeExamServiceHub.Controllers
{
    [Route("api/[controller]")]
    public class ClgExamMasterFacadeController : Controller
    {
        ClgExamMasterInterface inter;
        public ClgExamMasterFacadeController(ClgExamMasterInterface obj)
        {
            inter = obj;
        }

        [Route("Getdetails")]
        public exammasterDTO Getdetails([FromBody]exammasterDTO data)
        {
            return inter.Getdetails(data);
        }
        [Route("savedetails")]
        public exammasterDTO savedetails([FromBody] exammasterDTO data)
        {
            return inter.savedetails(data);
        }

        [Route("editdetails/{id:int}")]
        public exammasterDTO editdetails(int ID)
        {
            return inter.editdetails(ID);
        }

        [Route("validateordernumber")]
        public exammasterDTO validateordernumber([FromBody] exammasterDTO data)
        {
            return inter.validateordernumber(data);
        }

        [Route("deactivate")]
        public exammasterDTO deactivate([FromBody] exammasterDTO data)
        {
            return inter.deactivate(data);
        }
    }
}
