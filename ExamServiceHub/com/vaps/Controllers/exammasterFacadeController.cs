
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
    public class exammasterFacadeController : Controller
    {
        public exammasterInterface _exammaster;

        public exammasterFacadeController(exammasterInterface exammaster)
        {
            _exammaster = exammaster;
        }


        [Route("Getdetails")]
        public exammasterDTO Getdetails([FromBody]exammasterDTO data)//int IVRMM_Id
        {
           
            return _exammaster.Getdetails(data);
           
        }

        [Route("editdetails/{id:int}")]
        public exammasterDTO editdetails(int ID)
        {
            return _exammaster.editdetails(ID);
        }
        
        [Route("validateordernumber")]
        public exammasterDTO validateordernumber([FromBody] exammasterDTO data)
        {
            return _exammaster.validateordernumber(data);
        }

        [Route("savedetails")]
        public exammasterDTO savedetails([FromBody] exammasterDTO data)
        {
            return _exammaster.savedetails(data);
        }
       
        [Route("deactivate")]
        public exammasterDTO deactivate([FromBody] exammasterDTO data)
        {           
            return _exammaster.deactivate(data);
        }

        // Master Exam Paper Type
        [Route("BindData_PaperType")]
        public exammasterDTO BindData_PaperType([FromBody] exammasterDTO data)
        {           
            return _exammaster.BindData_PaperType(data);
        }

        [Route("Saveddata_PT")]
        public exammasterDTO Saveddata_PT([FromBody] exammasterDTO data)
        {
            return _exammaster.Saveddata_PT(data);
        }

        [Route("Editdata_PT")]
        public exammasterDTO Editdata_PT([FromBody] exammasterDTO data)
        {
            return _exammaster.Editdata_PT(data);
        }

        [Route("DeactivateActivateMasterExam_PT")]
        public exammasterDTO DeactivateActivateMasterExam_PT([FromBody] exammasterDTO data)
        {
            return _exammaster.DeactivateActivateMasterExam_PT(data);
        }

    }
}
