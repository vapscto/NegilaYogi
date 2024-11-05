
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
    public class MasterScholasticActivityFacadeController : Controller
    {
        public MasterScholasticActivityInterface _MasterScholasticActivity;

        public MasterScholasticActivityFacadeController(MasterScholasticActivityInterface MasterScholasticActivity)
        {
            _MasterScholasticActivity = MasterScholasticActivity;
        }


        [Route("Getdetails")]
        public MasterScholasticActivityDTO Getdetails([FromBody]MasterScholasticActivityDTO data)//int IVRMM_Id
        {
           
            return _MasterScholasticActivity.Getdetails(data);
           
        }

        [Route("editdetails/{id:int}")]
        public MasterScholasticActivityDTO editdetails(int ID)
        {
            return _MasterScholasticActivity.editdetails(ID);
        }
        

        [Route("savedata")]
        public MasterScholasticActivityDTO savedata([FromBody] MasterScholasticActivityDTO data)
        {
            return _MasterScholasticActivity.savedata(data);
        }
       
        [Route("deactivate")]
        public MasterScholasticActivityDTO deactivate([FromBody] MasterScholasticActivityDTO data)
        {           
            return _MasterScholasticActivity.deactivate(data);
        }

        [Route("validateordernumber")]
        public MasterScholasticActivityDTO validateordernumber([FromBody] MasterScholasticActivityDTO data)
        {
            return _MasterScholasticActivity.validateordernumber(data);
        }

    }
}
