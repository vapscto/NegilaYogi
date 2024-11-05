using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class ExamPromotionReportFacade : Controller
    {

        public ExamPromotionReportInterface _intf;

        public ExamPromotionReportFacade(ExamPromotionReportInterface para)
        {
            _intf = para;
        }

        [Route("Getdetails")]
        public ExamPromotionReport_DTO Getdetails([FromBody] ExamPromotionReport_DTO data)
        {
            return _intf.Getdetails(data);
        }
        [Route("get_class")]
        public ExamPromotionReport_DTO get_class([FromBody] ExamPromotionReport_DTO data)
        {
            return _intf.get_class(data);
        }
        [Route("get_section")]
        public ExamPromotionReport_DTO get_section([FromBody] ExamPromotionReport_DTO data)
        {
            return _intf.get_section(data);
        }
        [Route("get_exam")]
        public ExamPromotionReport_DTO get_exam([FromBody] ExamPromotionReport_DTO data)
        {
            return _intf.get_exam(data);
        }
        [Route("get_reports")]
        public Task<ExamPromotionReport_DTO> get_reports([FromBody] ExamPromotionReport_DTO data)
        {
            return _intf.get_reports(data);
        }
    }
}
