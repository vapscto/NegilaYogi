
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
    public class exammasterRemarkFacadeController : Controller
    {
        public exammasterRemarkInterface _exammasterremark;

        public exammasterRemarkFacadeController(exammasterRemarkInterface exammasterremark)
        {
            _exammasterremark = exammasterremark;
        }


        [Route("Getdetails")]
        public exammasterRemarkDTO Getdetails([FromBody]exammasterRemarkDTO data)
        {
           
            return _exammasterremark.Getdetails(data);
           
        }

        [Route("editdetails/{id:int}")]
        public exammasterRemarkDTO editdetails(int ID)
        {
            return _exammasterremark.editdetails(ID);
        }
        
        [Route("validateordernumber")]
        public exammasterRemarkDTO validateordernumber([FromBody] exammasterRemarkDTO data)
        {
            return _exammasterremark.validateordernumber(data);
        }

        [Route("savedetails")]
        public exammasterRemarkDTO savedetails([FromBody] exammasterRemarkDTO data)
        {
            return _exammasterremark.savedetails(data);
        }
       
        [Route("deactivate")]
        public exammasterRemarkDTO deactivate([FromBody] exammasterRemarkDTO data)
        {           
            return _exammasterremark.deactivate(data);
        }

        //Exam Wise Student Remarks Entry

        [Route("studentdataload")]
        public exammasterRemarkDTO studentdataload([FromBody]exammasterRemarkDTO data)
        {
            return _exammasterremark.studentdataload(data);
        }

        [Route("onchangeyear")]
        public exammasterRemarkDTO onchangeyear([FromBody]exammasterRemarkDTO data)
        {
            return _exammasterremark.onchangeyear(data);
        }

        [Route("onchangeclass")]
        public exammasterRemarkDTO onchangeclass([FromBody]exammasterRemarkDTO data)
        {
            return _exammasterremark.onchangeclass(data);
        }

        [Route("onchangesection")]
        public exammasterRemarkDTO onchangesection([FromBody]exammasterRemarkDTO data)
        {
            return _exammasterremark.onchangesection(data);
        }

        [Route("searchdata")]
        public exammasterRemarkDTO searchdata([FromBody]exammasterRemarkDTO data)
        {
            return _exammasterremark.searchdata(data);
        }

        [Route("savemapping")]
        public exammasterRemarkDTO savemapping([FromBody]exammasterRemarkDTO data)
        {
            return _exammasterremark.savemapping(data);
        }

        [Route("editmappingdetails")]
        public exammasterRemarkDTO editmappingdetails([FromBody]exammasterRemarkDTO data)
        {
            return _exammasterremark.editmappingdetails(data);
        }

        [Route("ViewSubjectWiseRemarks")]
        public exammasterRemarkDTO ViewSubjectWiseRemarks([FromBody]exammasterRemarkDTO data)
        {
            return _exammasterremark.ViewSubjectWiseRemarks(data);
        }

        //Subject Wise Remarks
        [Route("Subjectwise_studentdataload")]
        public exammasterRemarkDTO Subjectwise_studentdataload([FromBody]exammasterRemarkDTO data)
        {
            return _exammasterremark.Subjectwise_studentdataload(data);
        }

        [Route("Subjectwise_onchangeyear")]
        public exammasterRemarkDTO Subjectwise_onchangeyear([FromBody]exammasterRemarkDTO data)
        {
            return _exammasterremark.Subjectwise_onchangeyear(data);
        }

        [Route("Subjectwise_onchangeclass")]
        public exammasterRemarkDTO Subjectwise_onchangeclass([FromBody]exammasterRemarkDTO data)
        {
            return _exammasterremark.Subjectwise_onchangeclass(data);
        }

        [Route("Subjectwise_onchangesection")]
        public exammasterRemarkDTO Subjectwise_onchangesection([FromBody]exammasterRemarkDTO data)
        {
            return _exammasterremark.Subjectwise_onchangesection(data);
        }

        [Route("Subjectwise_onchangeexam")]
        public exammasterRemarkDTO Subjectwise_onchangeexam([FromBody]exammasterRemarkDTO data)
        {
            return _exammasterremark.Subjectwise_onchangeexam(data);
        }

        [Route("Subjectwise_searchdata")]
        public exammasterRemarkDTO Subjectwise_searchdata([FromBody]exammasterRemarkDTO data)
        {
            return _exammasterremark.Subjectwise_searchdata(data);
        }

        [Route("SubjectWise_savemapping")]
        public exammasterRemarkDTO SubjectWise_savemapping([FromBody]exammasterRemarkDTO data)
        {
            return _exammasterremark.SubjectWise_savemapping(data);
        }

        [Route("SubjectWise_editmappingdetails")]
        public exammasterRemarkDTO SubjectWise_editmappingdetails([FromBody]exammasterRemarkDTO data)
        {
            return _exammasterremark.SubjectWise_editmappingdetails(data);
        }

    }
}
