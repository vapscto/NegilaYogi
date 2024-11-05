
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
    public class PromotionCalculationFacadeController : Controller
    {
        public PromotionCalculationInterface _CumulativeReport;

        public PromotionCalculationFacadeController(PromotionCalculationInterface data)
        {
            _CumulativeReport = data;
        }

        [Route("Getdetails")]
        public PromotionCalculationDTO Getdetails([FromBody]PromotionCalculationDTO data)//int IVRMM_Id
        {           
            return _CumulativeReport.Getdetails(data);           
        }

        [HttpPost]
        [Route("get_cls_sections")]
        public PromotionCalculationDTO get_cls_sections([FromBody] PromotionCalculationDTO org)
        {
            return _CumulativeReport.get_cls_sections(org);
        }

         [Route("Calculation")]
        public PromotionCalculationDTO Calculation([FromBody] PromotionCalculationDTO org)
        {
            return _CumulativeReport.Calculation(org);
        }

        [Route("get_classes")]
        public PromotionCalculationDTO get_classes([FromBody] PromotionCalculationDTO id)
        {
            return _CumulativeReport.get_classes(id);
        }

        [Route("publishtostudentportal")]
        public PromotionCalculationDTO publishtostudentportal([FromBody] PromotionCalculationDTO id)
        {
            return _CumulativeReport.publishtostudentportal(id);
        }

        [Route("onchangesection")]
        public PromotionCalculationDTO onchangesection([FromBody] PromotionCalculationDTO id)
        {
            return _CumulativeReport.onchangesection(id);
        }
    }
}
