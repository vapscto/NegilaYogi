using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdmissionServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.admission;
using DomainModel.Model;
using DataAccessMsSqlServerProvider;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using Microsoft.EntityFrameworkCore;

namespace AdmissionServiceHub.com.vaps.Controllers
{

    [Route("api/[controller]")]
    public class StudentAddressBook2FacadeController:Controller
    {
        private StudentAddressBook2Interface _sad;
        public StudentAddressBook2FacadeController(StudentAddressBook2Interface sad)
        {
            _sad = sad;
        }

        [HttpGet]
        [Route("getinitialdata/{id:int}")]
        public Task<StudentAddressBook2DTO> getInitialData(int id)
        {
            return _sad.getInitailData(id);
        }

        [HttpPost]
        [Route("getdetails")]
        //[Route("getenquirycontroller")]
        public async Task<StudentAddressBook2DTO> getdetails([FromBody] StudentAddressBook2DTO data)
        {           
            return await _sad.getdetails(data);
        }
        [Route("getdetailsstdemp")]      
        public StudentAddressBook2DTO getdetailsstdemp([FromBody] StudentAddressBook2DTO data)
        {
            return _sad.getdetailsstdemp(data);
        }        

        [Route("classchange/{id:int}")]
        public Task<StudentAddressBook2DTO> classchange(StudentAddressBook2DTO id)
        {
            return _sad.classchange(id);
        }

        [Route("yearchange")]
        public Task<StudentAddressBook2DTO> yearchange([FromBody]StudentAddressBook2DTO id)
        {
            return _sad.yearchange(id);
        }

        [Route("sectionchange")]
        public Task<StudentAddressBook2DTO> sectionchange([FromBody]StudentAddressBook2DTO dto)
        {
            return _sad.sectionchange(dto);
        }

        [Route("ExportToExcle/")]
        public string ExportToExcle([FromBody] StudentAddressBook2DTO reg)
        {
            string str = "";
            return str;
        }

        [Route("yearchangenew")]
        public StudentAddressBook2DTO yearchangenew([FromBody]StudentAddressBook2DTO MMD)
        { 
            return _sad.yearchangenew(MMD);
        }
        [Route("classchangenew")]
        public StudentAddressBook2DTO classchangenew([FromBody]StudentAddressBook2DTO MMD)
        { 
            return _sad.classchangenew(MMD);
        }
        [Route("sectionchangenew")]
        public StudentAddressBook2DTO sectionchangenew([FromBody]StudentAddressBook2DTO MMD)
        { 
            return _sad.sectionchangenew(MMD);
        }
        [Route("getdetailsnew")]
        public StudentAddressBook2DTO getdetailsnew([FromBody]StudentAddressBook2DTO MMD)
        { 
            return _sad.getdetailsnew(MMD);
        }
    }
}
