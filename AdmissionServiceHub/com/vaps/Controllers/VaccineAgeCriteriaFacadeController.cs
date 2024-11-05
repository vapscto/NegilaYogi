using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdmissionServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdmissionServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class VaccineAgeCriteriaFacadeController : Controller
    {
        public VaccineAgeCriteriaInterface _interface;
        public VaccineAgeCriteriaFacadeController(VaccineAgeCriteriaInterface _inter)
        {
            _interface = _inter;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("OnLoadVaccineAgeCriteriaDetails")]
        public VaccineAgeCriteriaDTO OnLoadVaccineAgeCriteriaDetails([FromBody] VaccineAgeCriteriaDTO data)
        { 
            return _interface.OnLoadVaccineAgeCriteriaDetails(data);
        }

        [Route("SaveVaccineAgeDetails")]
        public VaccineAgeCriteriaDTO SaveVaccineAgeDetails([FromBody] VaccineAgeCriteriaDTO data)
        {
            return _interface.SaveVaccineAgeDetails(data);
        }

        [Route("EditVaccineAgeDetails")]
        public VaccineAgeCriteriaDTO EditVaccineAgeDetails([FromBody] VaccineAgeCriteriaDTO data)
        {
            return _interface.EditVaccineAgeDetails(data);
        }

        [Route("ActiveDeactiveVaccineAgeDetails")]
        public VaccineAgeCriteriaDTO ActiveDeactiveVaccineAgeDetails([FromBody] VaccineAgeCriteriaDTO data)
        {
            return _interface.ActiveDeactiveVaccineAgeDetails(data);
        }

        [Route("OnClickViewDetails")]
        public VaccineAgeCriteriaDTO OnClickViewDetails([FromBody] VaccineAgeCriteriaDTO data)
        {
            return _interface.OnClickViewDetails(data);
        }

        [Route("ActiveDeactiveVaccineDetails")]
        public VaccineAgeCriteriaDTO ActiveDeactiveVaccineDetails([FromBody] VaccineAgeCriteriaDTO data)
        {
            return _interface.ActiveDeactiveVaccineDetails(data);
        }

        // Vaccine Student Details

        [Route("OnLoadVaccineStudentDetails")]
        public VaccineAgeCriteriaDTO OnLoadVaccineStudentDetails([FromBody] VaccineAgeCriteriaDTO data)
        {
            return _interface.OnLoadVaccineStudentDetails(data);
        }

        [Route("GetStudentDetailsBySearch")]
        public VaccineAgeCriteriaDTO GetStudentDetailsBySearch([FromBody] VaccineAgeCriteriaDTO data)
        {
            return _interface.GetStudentDetailsBySearch(data);
        }

        [Route("SearchVaccineStudentDetails")]
        public VaccineAgeCriteriaDTO SearchVaccineStudentDetails([FromBody] VaccineAgeCriteriaDTO data)
        {
            return _interface.SearchVaccineStudentDetails(data);
        }

        [Route("SaveStudentVaccineDetails")]
        public VaccineAgeCriteriaDTO SaveStudentVaccineDetails([FromBody] VaccineAgeCriteriaDTO data)
        {
            return _interface.SaveStudentVaccineDetails(data);
        }

        [Route("OnClickViewStudentVaccineDetails")]
        public VaccineAgeCriteriaDTO OnClickViewStudentVaccineDetails([FromBody] VaccineAgeCriteriaDTO data)
        {
            return _interface.OnClickViewStudentVaccineDetails(data);
        }

        // Webjobs Api

        [Route("VaccineDueDateWebJobsApi")]
        public Task<VaccineAgeCriteriaDTO> VaccineDueDateWebJobsApi([FromBody] VaccineAgeCriteriaDTO data)
        {
            return _interface.VaccineDueDateWebJobsApi(data);
        }
        

        [Route("OnLoadIllnessStudentDetails")]
        public VaccineAgeCriteriaDTO OnLoadIllnessStudentDetails([FromBody] VaccineAgeCriteriaDTO data)
        {
            return _interface.OnLoadIllnessStudentDetails(data);
        }

    }
}