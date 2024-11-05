using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;
using AdmissionServiceHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AdmissionServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class StudentTCFacadeController : Controller
    {
        public StudentTCInterface _IAtt;
        public StudentTCFacadeController(StudentTCInterface enqui)
        {
            _IAtt = enqui;
        }
        // GET: api/values
        [HttpPost]

        [Route("getactiveDetails")]
        public StudentTCDTO getactiveDetails([FromBody]StudentTCDTO tc)
        {
            return _IAtt.gettcDetails(tc);
        }

        [Route("getStatusDetails")]
        public StudentTCDTO getStatusDetails([FromBody]StudentTCDTO tc)
        {
            return _IAtt.getStatusDetails(tc);
        }

        [Route("savedetails")]
        public Task<StudentTCDTO> Post([FromBody]StudentTCDTO stu_tc)
        {
            return _IAtt.saveTcdet(stu_tc);
        }

        [Route("chk_tc_dup")]
        public StudentTCDTO chk_tc_dup([FromBody]StudentTCDTO stu_tc)
        {
            return _IAtt.chk_tc_dup(stu_tc);
        }

        [Route("getstudent_name_list")]
        public async Task<StudentTCDTO> getstudent_name_list([FromBody]StudentTCDTO name_list)
        {
            return await _IAtt.getstudent_name_list(name_list);
        }

        [Route("getstudenttcdata")]
        public async Task<StudentTCDTO> getstudenttcdata([FromBody]StudentTCDTO MIID)
        {
            return await _IAtt.GetStudentInitialData(MIID);
        }

        [Route("gettcDetails")]
        public StudentTCDTO gettcDetails([FromBody]StudentTCDTO tc)
        {
            return _IAtt.gettcDetails(tc);
        }

        [Route("searchfilter")]
        public StudentTCDTO searchfilter([FromBody]StudentTCDTO data)
        {
            return _IAtt.searchfilter(data);
        }

        // TC Cancel
        [Route("GetTCCancelDetails")]
        public StudentTCDTO GetTCCancelDetails([FromBody]  StudentTCDTO data)
        {
            return _IAtt.GetTCCancelDetails(data);
        }
        [Route("OnChangeAcademicYear")]
        public StudentTCDTO OnChangeAcademicYear([FromBody]  StudentTCDTO data)
        {
            return _IAtt.OnChangeAcademicYear(data);
        }
        [Route("OnStudentNameChange")]
        public StudentTCDTO OnStudentNameChange([FromBody]  StudentTCDTO data)
        {
            return _IAtt.OnStudentNameChange(data);
        }
        [Route("SaveTCCancelDetails")]
        public StudentTCDTO SaveTCCancelDetails([FromBody]  StudentTCDTO data)
        {
            return _IAtt.SaveTCCancelDetails(data);
        }

        //
        [Route("sourcecntdata")]
        public Task<StudentTCDTO> radiobtndata([FromBody] StudentTCDTO data)
        {
            return _IAtt.sourcecntdata(data);
        }
        [Route("getallsourcedetails")]
        public StudentTCDTO getallsourcedetails([FromBody]  StudentTCDTO data)
        {
            return _IAtt.getallsourcedetails(data);
        }

        //MotherTongueWise
        [Route("languagecntdata")]
        public Task<StudentTCDTO> languagecntdata([FromBody] StudentTCDTO data)
        {
            return _IAtt.languagecntdata(data);
        }
        [Route("statecntdata")]
        public Task<StudentTCDTO> statecntdata([FromBody] StudentTCDTO data)
        {
            return _IAtt.statecntdata(data);
        }
        

    }
}
