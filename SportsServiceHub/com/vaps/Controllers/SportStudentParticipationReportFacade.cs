using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Sports;
using SportsServiceHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SportsServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class SportStudentParticipationReportFacade : Controller
    {
        public SportStudentParticipationReportInterface _ReportContext;

        public SportStudentParticipationReportFacade(SportStudentParticipationReportInterface data)
        {
            _ReportContext = data;
        }

        [Route("Getdetails")]
        public StudentAgeCalcReport_DTO Getdetails([FromBody]StudentAgeCalcReport_DTO data)//int IVRMM_Id
        {

            return _ReportContext.Getdetails(data);
        }


        [Route("showdetails")]
        public Task<StudentAgeCalcReport_DTO> showdetails([FromBody] StudentAgeCalcReport_DTO data)
        {
            return _ReportContext.showdetails(data);
        }

        [Route("get_class")]
        public StudentAgeCalcReport_DTO get_class([FromBody] StudentAgeCalcReport_DTO data)
        {
            return _ReportContext.get_class(data);
        }

        [Route("get_section")]
        public StudentAgeCalcReport_DTO get_section([FromBody] StudentAgeCalcReport_DTO data)
        {
            return _ReportContext.get_section(data);
        }




        #region
        //[Route("Getdetails")]
        //public SportStudentParticipationReportDTO Getdetails([FromBody]SportStudentParticipationReportDTO data)//int IVRMM_Id
        //{

        //    return _ReportContext.Getdetails(data);
        //}


        //[Route("showdetails")]
        //public SportStudentParticipationReportDTO showdetails([FromBody] SportStudentParticipationReportDTO data)
        //{
        //    return _ReportContext.showdetails(data);
        //}
        //[Route("getevent")]
        //public SportStudentParticipationReportDTO getevent([FromBody] SportStudentParticipationReportDTO data)
        //{
        //    return _ReportContext.getevent(data);
        //}

        //[Route("get_class")]
        //public SportStudentParticipationReportDTO get_class([FromBody] SportStudentParticipationReportDTO data)
        //{
        //    return _ReportContext.get_class(data);
        //}
        //[Route("get_section")]
        //public SportStudentParticipationReportDTO get_section([FromBody] SportStudentParticipationReportDTO data)
        //{
        //    return _ReportContext.get_section(data);
        //}
        //[Route("get_student/")]
        //public SportStudentParticipationReportDTO get_student([FromBody] SportStudentParticipationReportDTO data)//int IVRMM_Id
        //{

        //    return _ReportContext.get_student(data);

        //}

        #endregion

    }
}
