
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
    public class ExamCalculation_SSSEFacadeController : Controller
    {
        public ExamCalculation_SSSEInterface _CumulativeReport;

        public ExamCalculation_SSSEFacadeController(ExamCalculation_SSSEInterface data)
        {
            _CumulativeReport = data;
        }        

        [Route("Getdetails")]
        public ExamCalculation_SSSEDTO Getdetails([FromBody]ExamCalculation_SSSEDTO data)//int IVRMM_Id
        {           
            return _CumulativeReport.Getdetails(data);           
        }

        [HttpPost]
        [Route("get_cls_sections")]
        public ExamCalculation_SSSEDTO get_cls_sections([FromBody] ExamCalculation_SSSEDTO org)
        {
            return _CumulativeReport.get_cls_sections(org);
        }

         [Route("Calculation")]
        public ExamCalculation_SSSEDTO Calculation([FromBody] ExamCalculation_SSSEDTO org)
        {
            return _CumulativeReport.Calculation(org);
        }

        [Route("get_classes")]
        public ExamCalculation_SSSEDTO get_classes([FromBody] ExamCalculation_SSSEDTO id)
        {
            return _CumulativeReport.get_classes(id);
        }

        [Route("get_exams")]
        public ExamCalculation_SSSEDTO get_exams([FromBody] ExamCalculation_SSSEDTO id)
        {
            return _CumulativeReport.get_exams(id);
        }

        [Route("onchangeexam")]
        public ExamCalculation_SSSEDTO onchangeexam([FromBody] ExamCalculation_SSSEDTO id)
        {
            return _CumulativeReport.onchangeexam(id);
        }              

        [Route("saveapporvecal")]
        public ExamCalculation_SSSEDTO saveapporvecal([FromBody] ExamCalculation_SSSEDTO id)
        {
            return _CumulativeReport.saveapporvecal(id);
        }

        // Student Wise Publish 
        [Route("ChangeOfSection")]
        public ExamCalculation_SSSEDTO ChangeOfSection([FromBody] ExamCalculation_SSSEDTO id)
        {
            return _CumulativeReport.ChangeOfSection(id);
        }

        [Route("CheckMarksCalculated")]
        public ExamCalculation_SSSEDTO CheckMarksCalculated([FromBody] ExamCalculation_SSSEDTO id)
        {
            return _CumulativeReport.CheckMarksCalculated(id);
        }

        [Route("SearchStudent")]
        public ExamCalculation_SSSEDTO SearchStudent([FromBody] ExamCalculation_SSSEDTO id)
        {
            return _CumulativeReport.SearchStudent(id);
        }

        [Route("SaveStudentStatus")]
        public ExamCalculation_SSSEDTO SaveStudentStatus([FromBody] ExamCalculation_SSSEDTO id)
        {
            return _CumulativeReport.SaveStudentStatus(id);
        }

        // Promotion

        [Route("onchangesection")]
        public ExamCalculation_SSSEDTO onchangesection([FromBody] ExamCalculation_SSSEDTO id)
        {
            return _CumulativeReport.onchangesection(id);
        }

        [Route("promotionsaveddata")]
        public ExamCalculation_SSSEDTO promotionsaveddata([FromBody] ExamCalculation_SSSEDTO id)
        {
            return _CumulativeReport.promotionsaveddata(id);
        }

        [Route("publishtostudentportal")]
        public ExamCalculation_SSSEDTO publishtostudentportal([FromBody] ExamCalculation_SSSEDTO id)
        {
            return _CumulativeReport.publishtostudentportal(id);
        }
        
    }
}
