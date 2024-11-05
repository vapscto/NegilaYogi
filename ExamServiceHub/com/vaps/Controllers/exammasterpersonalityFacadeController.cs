
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
    public class exammasterpersonalityFacadeController : Controller
    {
        public exammasterPersonalityInterface _exammasterpersonality;

        public exammasterpersonalityFacadeController(exammasterPersonalityInterface exammaster)
        {
            _exammasterpersonality = exammaster;
        }

        // Master Personlity

        [Route("Getdetails")]
        public exammasterpersonalityDTO Getdetails([FromBody]exammasterpersonalityDTO data)//int IVRMM_Id
        {
           
            return _exammasterpersonality.Getdetails(data);
           
        }

        [Route("editdetails/{id:int}")]
        public exammasterpersonalityDTO editdetails(int ID)
        {
            return _exammasterpersonality.editdetails(ID);
        }
        
        [Route("validateordernumber")]
        public exammasterpersonalityDTO validateordernumber([FromBody] exammasterpersonalityDTO data)
        {
            return _exammasterpersonality.validateordernumber(data);
        }

        [Route("savedetails")]
        public exammasterpersonalityDTO savedetails([FromBody] exammasterpersonalityDTO data)
        {
            return _exammasterpersonality.savedetails(data);
        }
       
        [Route("deactivate")]
        public exammasterpersonalityDTO deactivate([FromBody] exammasterpersonalityDTO data)
        {           
            return _exammasterpersonality.deactivate(data);
        }

        //Student Mapping Personlity

        [Route("studentdataload")]
        public exammasterpersonalityDTO studentdataload([FromBody]exammasterpersonalityDTO data)//int IVRMM_Id
        {
            return _exammasterpersonality.studentdataload(data);
        }
        [Route("onchangeyear")]
        public exammasterpersonalityDTO onchangeyear([FromBody]exammasterpersonalityDTO data)//int IVRMM_Id
        {
            return _exammasterpersonality.onchangeyear(data);
        }
        [Route("onchangeclass")]
        public exammasterpersonalityDTO onchangeclass([FromBody]exammasterpersonalityDTO data)//int IVRMM_Id
        {
            return _exammasterpersonality.onchangeclass(data);
        }
        [Route("onchangesection")]
        public exammasterpersonalityDTO onchangesection([FromBody]exammasterpersonalityDTO data)//int IVRMM_Id
        {
            return _exammasterpersonality.onchangesection(data);
        }
        [Route("searchdata")]
        public exammasterpersonalityDTO searchdata([FromBody]exammasterpersonalityDTO data)//int IVRMM_Id
        {
            return _exammasterpersonality.searchdata(data);
        }
        [Route("savemapping")]
        public exammasterpersonalityDTO savemapping([FromBody]exammasterpersonalityDTO data)//int IVRMM_Id
        {
            return _exammasterpersonality.savemapping(data);
        }
        [Route("editmappingdetails")]
        public exammasterpersonalityDTO editmappingdetails([FromBody]exammasterpersonalityDTO data)//int IVRMM_Id
        {
            return _exammasterpersonality.editmappingdetails(data);
        }
        
    }
}
