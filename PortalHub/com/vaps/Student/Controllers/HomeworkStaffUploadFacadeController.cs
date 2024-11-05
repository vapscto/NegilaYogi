using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortalHub.com.vaps.Student.Interfaces;
using PreadmissionDTOs.com.vaps.Portals.Student;

namespace PortalHub.com.vaps.Student.Controllers
{
    [Produces("application/json")]
    [Route("api/HomeworkStaffUploadFacade")]
    public class HomeworkStaffUploadFacadeController : Controller
    {
        public HomeworkstaffUploadReportInterface _inter;
        public HomeworkStaffUploadFacadeController(HomeworkstaffUploadReportInterface inter)
        {
            _inter = inter;
        }
        [Route("getAllDetail")]
        public HomeworkStaffReportDTO getAllDetail([FromBody]HomeworkStaffReportDTO dto)
        {
            return _inter.getAllDetail(dto);           
        }
        [Route("get_load_onchange")]
        public HomeworkStaffReportDTO get_load_onchange([FromBody]HomeworkStaffReportDTO dto)
        {
            return _inter.get_load_onchange(dto);
        }
        [Route("getReport")]
        public HomeworkStaffReportDTO getReport([FromBody]HomeworkStaffReportDTO dto)
        {
            return _inter.getReport(dto);
        }
        //getOnchange
        [Route("getOnchange")]
        public HomeworkStaffReportDTO getOnchange([FromBody]HomeworkStaffReportDTO dto)
        {
            return _inter.getOnchange(dto);
        }
        //------Class Wise-----//
        [Route("getloadDetails")]
          public HomeworkStaffReportDTO getloadDetails([FromBody]HomeworkStaffReportDTO dto)
        {
            return _inter.getloadDetails(dto);
        }
        [Route("getLoad_onchange")]

        public HomeworkStaffReportDTO getLoad_onchange([FromBody]HomeworkStaffReportDTO dto)
        {
            return _inter.getLoad_onchange(dto);
        }

        [Route("getReport_classwise")]
        public HomeworkStaffReportDTO getReport_classwise([FromBody]HomeworkStaffReportDTO dto)
        {
            return _inter.getReport_classwise(dto);
        }
        //smsemail
        [Route("smsemail")]
        public HomeworkStaffReportDTO smsemail([FromBody]HomeworkStaffReportDTO dto)
        {
            return _inter.smsemail(dto);
        }
        [Route("getOnchangeclass")]
        public HomeworkStaffReportDTO getOnchangeclass([FromBody]HomeworkStaffReportDTO dto)
        {
            return _inter.getOnchangeclass(dto);
        }
    }
}