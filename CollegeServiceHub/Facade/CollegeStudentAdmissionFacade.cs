using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CollegeServiceHub.Interface;
using PreadmissionDTOs.com.vaps.College.Admission;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeServiceHub.Facade
{
    [Route("api/[controller]")]
    public class CollegeStudentAdmissionFacade : Controller
    {
        CollegeStudentAdmissionInterface _interface;
        public CollegeStudentAdmissionFacade(CollegeStudentAdmissionInterface interf)
        {
            _interface = interf;
        }
        [Route("Getdetails")]
        public AdmMasterCollegeStudentDTO Getdetails([FromBody]AdmMasterCollegeStudentDTO obj)
        {
            return _interface.Getdetails(obj);
        }
        [Route("getCourse")]
        public AdmMasterCollegeStudentDTO getCourse([FromBody]AdmMasterCollegeStudentDTO data)
        {
            return _interface.getCourse(data);
        }
        [Route("getBranch")]
        public AdmMasterCollegeStudentDTO getBranch([FromBody] AdmMasterCollegeStudentDTO dt)
        {
            return _interface.getBranch(dt);
        }
        [Route("getSemester")]
        public AdmMasterCollegeStudentDTO getSemester([FromBody] AdmMasterCollegeStudentDTO dto)
        {
            return _interface.getSemester(dto);
        }
        [Route("getCaste")]
        public AdmMasterCollegeStudentDTO getCaste([FromBody] AdmMasterCollegeStudentDTO dtos)
        {
            return _interface.getcaste(dtos);
        }
        [Route("getQuotaCategory")]
        public AdmMasterCollegeStudentDTO getQuotaCategory([FromBody] AdmMasterCollegeStudentDTO obj)
        {
            return _interface.getQuotaCategory(obj);
        }
        [Route("saveStudentDetails")]
        public Task<save_firsttab_details> saveStudentDetails([FromBody] save_firsttab_details obj)
        {
            return _interface.saveStudentDetails(obj);
        }
        [Route("Edit")]
        public AdmMasterCollegeStudentDTO Edit([FromBody] AdmMasterCollegeStudentDTO editdata)
        {
            return _interface.Edit(editdata);
        }
        [Route("checkDuplicate")]
        public AdmMasterCollegeStudentDTO checkDuplicate([FromBody] AdmMasterCollegeStudentDTO dup)
        {
            return _interface.checkDuplicate(dup);
        }
        [Route("getdpstate")]
        public AdmMasterCollegeStudentDTO getdpstate([FromBody] AdmMasterCollegeStudentDTO dups)
        {
            return _interface.getdpstate(dups);
        }
        [Route("saveAddress")]
        public AdmMasterCollegeStudentDTO saveAddress([FromBody] AdmMasterCollegeStudentDTO addr)
        {
            return _interface.saveAddress(addr);
        }
        [Route("saveParentsDetails")]
        public AdmMasterCollegeStudentDTO saveParentsDetails([FromBody] AdmMasterCollegeStudentDTO ParentsData)
        {
            return _interface.saveParentsDetails(ParentsData);
        }
        [Route("StateByCountryName")]
        public AdmMasterCollegeStudentDTO StateByCountryName([FromBody] AdmMasterCollegeStudentDTO country)
        {
            return _interface.StateByCountryName(country);
        }
        [Route("saveOthersDetails")]
        public AdmMasterCollegeStudentDTO saveOthersDetails([FromBody] AdmMasterCollegeStudentDTO others)
        {
            return _interface.saveOthersDetails(others);
        }
        [Route("saveDocuments")]
        public AdmMasterCollegeStudentDTO saveDocuments([FromBody] AdmMasterCollegeStudentDTO docs)
        {
            return _interface.saveDocuments(docs);
        }
        [Route("SearchByColumn")]
        public AdmMasterCollegeStudentDTO SearchByColumn([FromBody] AdmMasterCollegeStudentDTO docs)
        {
            return _interface.SearchByColumn(docs);
        }
        [Route("DeleteEntry")]
        public AdmMasterCollegeStudentDTO DeleteEntry([FromBody] AdmMasterCollegeStudentDTO docs)
        {
            return _interface.DeleteEntry(docs);
        }

        [Route("ViewStudentProfile")]
        public AdmMasterCollegeStudentDTO ViewStudentProfile([FromBody] AdmMasterCollegeStudentDTO data)
        {
            return _interface.ViewStudentProfile(data);
        }

        //master competitve exam
        [Route("compExamName")]
        public AdmMasterCollegeStudentDTO compExamName([FromBody] AdmMasterCollegeStudentDTO country)
        {
            return _interface.compExamName(country);
        }


        [Route("getprintdata")]
        public AdmMasterCollegeStudentDTO getprintdata([FromBody] AdmMasterCollegeStudentDTO editdata)
        {
            return _interface.getprintdata(editdata);
        }

        [Route("checkbiometriccode")]
        public AdmMasterCollegeStudentDTO checkbiometriccode([FromBody] AdmMasterCollegeStudentDTO data)
        {
            return _interface.checkbiometriccode(data);
        }

        [Route("checkrfcardduplicate")]
        public AdmMasterCollegeStudentDTO checkrfcardduplicate([FromBody] AdmMasterCollegeStudentDTO data)
        {
            return _interface.checkrfcardduplicate(data);
        }
    }
}
