
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
    public class ProgressCardReportFacadeController : Controller
    {
        public ProgressCardReportInterface _PCReportContext;

        public ProgressCardReportFacadeController(ProgressCardReportInterface data)
        {
            _PCReportContext = data;
        }


        [Route("Getdetails")]
        public ProgressCardReportDTO Getdetails([FromBody]ProgressCardReportDTO data)//int IVRMM_Id
        {
           
            return _PCReportContext.Getdetails(data);
           
        }

        [Route("editdetails/{id:int}")]
        public ProgressCardReportDTO editdetails(int ID)
        {
            return _PCReportContext.editdetails(ID);
        }
        
        [Route("validateordernumber")]
        public ProgressCardReportDTO validateordernumber([FromBody] ProgressCardReportDTO data)
        {
            return _PCReportContext.validateordernumber(data);
        }

        [Route("savedetails")]
        public ProgressCardReportDTO savedetails([FromBody] ProgressCardReportDTO data)
        {
            return _PCReportContext.savedetails(data);
        }
       
        [Route("deactivate")]
        public ProgressCardReportDTO deactivate([FromBody] ProgressCardReportDTO data)
        {           
            return _PCReportContext.deactivate(data);
        }     
       

    }
}
