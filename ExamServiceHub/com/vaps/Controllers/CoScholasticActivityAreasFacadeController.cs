
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ExamServiceHub.com.vaps.Interfaces;
//using AdmissionServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.Exam;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class CoScholasticActivityAreasFacadeController : Controller
    {
        public CoScholasticActivityAreasInterface _exammaster;

        public CoScholasticActivityAreasFacadeController(CoScholasticActivityAreasInterface exammaster)
        {
            _exammaster = exammaster;
        }


        [Route("Getdetails")]
        public CoScholasticActivityAreasDTO Getdetails([FromBody]CoScholasticActivityAreasDTO data)//int IVRMM_Id
        {

            return _exammaster.Getdetails(data);

        }

        [Route("editdetails/{id:int}")]
        public CoScholasticActivityAreasDTO editdetails(int ID)
        {
            return _exammaster.editdetails(ID);
        }

        [Route("validateordernumber")]
        public CoScholasticActivityAreasDTO validateordernumber([FromBody] CoScholasticActivityAreasDTO data)
        {
            return _exammaster.validateordernumber(data);
        }

        [Route("savedetails")]
        public CoScholasticActivityAreasDTO savedetails([FromBody] CoScholasticActivityAreasDTO data)
        {
            return _exammaster.savedetails(data);
        }

        [Route("deactivate")]
        public CoScholasticActivityAreasDTO deactivate([FromBody] CoScholasticActivityAreasDTO data)
        {
            return _exammaster.deactivate(data);
        }


    }
}
