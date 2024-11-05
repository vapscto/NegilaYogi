using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using AdmissionServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AdmissionServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class StudentAttendanceReportFacadeController : Controller
    {
        public StudentAtttendanceReportInterface _AttenRpt;
        public StudentAttendanceReportFacadeController(StudentAtttendanceReportInterface AttenRpt)
        {
            _AttenRpt = AttenRpt;
        }
        

        [Route("getinitialdata")]
        public StudentAttendanceReportDTO getinitialdataaa([FromBody] StudentAttendanceReportDTO data)
        {
            return _AttenRpt.getInitailData(data);
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

       
        [HttpPost]
        [Route("searchdata")]
        public  Task<StudentAttendanceReportDTO> searchdata([FromBody] StudentAttendanceReportDTO data)
        {
            return _AttenRpt.getserdata(data);
        }

        [Route("getdatatype")]
        public Task<StudentAttendanceReportDTO> getdatatype([FromBody] StudentAttendanceReportDTO data)
        {
            return _AttenRpt.getdatatype(data);
        }

        [Route("getdatabyselectedtype/{id:int}")]
        public StudentAttendanceReportDTO getDataByType(int id)
        {
            return _AttenRpt.getDataByTypeSelected(id);
        }

        [Route("getreportdiv")]
        public StudentAttendanceReportDTO getreportdiv([FromBody] StudentAttendanceReportDTO data)
        {
            return _AttenRpt.getreportdiv(data);
        }

        [Route("savetmpldatanew")]
        public StudentAttendanceReportDTO savetmpldatanew([FromBody] StudentAttendanceReportDTO data)
        {
            return _AttenRpt.savetmpldatanew(data);
        }

        [Route("onchangeyear")]
        public StudentAttendanceReportDTO onchangeyear([FromBody] StudentAttendanceReportDTO data)
        {
            return _AttenRpt.onchangeyear(data);
        }

        [Route("onclasschange")]
        public StudentAttendanceReportDTO onclasschange([FromBody] StudentAttendanceReportDTO data)
        {
            return _AttenRpt.onclasschange(data);
        }
        [Route("getclass")]
        public StudentAttendanceReportDTO getclass([FromBody] StudentAttendanceReportDTO data)
        {
            return _AttenRpt.getclass(data);
        }
        

        [Route("onsectionchange")]
        public StudentAttendanceReportDTO onsectionchange([FromBody] StudentAttendanceReportDTO data)
        {
            return _AttenRpt.onsectionchange(data);
        }

        [Route("getreport")]
        public StudentAttendanceReportDTO getreport([FromBody] StudentAttendanceReportDTO data)
        {
            return _AttenRpt.getreport(data);
        }

        // Subject wise attendance report

        [Route("LoadData")]
        public StudentAttendanceReportDTO LoadData([FromBody] StudentAttendanceReportDTO data)
        {
            return _AttenRpt.LoadData(data);
        }

        [Route("OnChangeAcademicYear")]
        public StudentAttendanceReportDTO OnChangeAcademicYear([FromBody] StudentAttendanceReportDTO data)
        {
            return _AttenRpt.OnChangeAcademicYear(data);
        }

        [Route("OnChangeClass")]
        public StudentAttendanceReportDTO OnChangeClass([FromBody] StudentAttendanceReportDTO data)
        {
            return _AttenRpt.OnChangeClass(data);
        }

        [Route("OnChangeSection")]
        public StudentAttendanceReportDTO OnChangeSection([FromBody] StudentAttendanceReportDTO data)
        {
            return _AttenRpt.OnChangeSection(data);
        }

        [Route("OnReport")]
        public StudentAttendanceReportDTO OnReport([FromBody] StudentAttendanceReportDTO data)
        {
            return _AttenRpt.OnReport(data);
        }

        [Route("PeriodWiseReportOverAll")]
        public StudentAttendanceReportDTO PeriodWiseReportOverAll([FromBody] StudentAttendanceReportDTO data)
        {
            return _AttenRpt.PeriodWiseReportOverAll(data);
        }

        [Route("OnAttendanceLoadData")]
        public StudentAttendanceReportDTO OnAttendanceLoadData([FromBody] StudentAttendanceReportDTO data)
        {
            return _AttenRpt.OnAttendanceLoadData(data);
        }

        [Route("OnAttendanceChangeYear")]
        public StudentAttendanceReportDTO OnAttendanceChangeYear([FromBody] StudentAttendanceReportDTO data)
        {
            return _AttenRpt.OnAttendanceChangeYear(data);
        }

        [Route("OnAttendanceChangeClass")]
        public StudentAttendanceReportDTO OnAttendanceChangeClass([FromBody] StudentAttendanceReportDTO data)
        {
            return _AttenRpt.OnAttendanceChangeClass(data);
        }

        [Route("OnAttendanceChangeSection")]
        public StudentAttendanceReportDTO OnAttendanceChangeSection([FromBody] StudentAttendanceReportDTO data)
        {
            return _AttenRpt.OnAttendanceChangeSection(data);
        }

        [Route("GetAttendanceDeletedReport")]
        public StudentAttendanceReportDTO GetAttendanceDeletedReport([FromBody] StudentAttendanceReportDTO data)
        {
            return _AttenRpt.GetAttendanceDeletedReport(data);
        }

        [Route("getstudetails")]
        public StudentAttendanceReportDTO getstudetails([FromBody] StudentAttendanceReportDTO data)
        {
            return _AttenRpt.getstudetails(data);
        }

        [Route("OnsendSMS")]
        public StudentAttendanceReportDTO OnsendSMS([FromBody] StudentAttendanceReportDTO data)
        {
            return _AttenRpt.OnsendSMS(data);
        }
        [Route("OnChangeSectionAbsent")]
        public StudentAttendanceReportDTO OnChangeSectionAbsent([FromBody] StudentAttendanceReportDTO data)
        {
            return _AttenRpt.OnChangeSectionAbsent(data);
        }
        [Route("OnChangeClassAbsent")]
        public StudentAttendanceReportDTO OnChangeClassAbsent([FromBody] StudentAttendanceReportDTO data)
        {
            return _AttenRpt.OnChangeClassAbsent(data);
        }

     
    }
}