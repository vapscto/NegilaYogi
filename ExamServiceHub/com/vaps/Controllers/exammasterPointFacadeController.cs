
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
    public class exammasterPointFacadeController : Controller
    {
        public exammasterPointInterface _exammasterInterface;

        public exammasterPointFacadeController(exammasterPointInterface exammasterInterface)
        {
            _exammasterInterface = exammasterInterface;
        }


        [Route("Getdetails")]
        public exammasterpointDTO Getdetails([FromBody]exammasterpointDTO data)//int IVRMM_Id
        {
           
            return _exammasterInterface.Getdetails(data);
           
        }

        [Route("editdetails/{id:int}")]
        public exammasterpointDTO editdetails(int ID)
        {
            return _exammasterInterface.editdetails(ID);
        }
        
        [Route("validateordernumber")]
        public exammasterpointDTO validateordernumber([FromBody] exammasterpointDTO data)
        {
            return _exammasterInterface.validateordernumber(data);
        }

        [Route("savedetails")]
        public exammasterpointDTO savedetails([FromBody] exammasterpointDTO data)
        {
            return _exammasterInterface.savedetails(data);
        }
       
        [Route("deactivate")]
        public exammasterpointDTO deactivate([FromBody] exammasterpointDTO data)
        {           
            return _exammasterInterface.deactivate(data);
        }     
       

    }
}
