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
    public class StudentAddressBook1FacadeController : Controller
    {
        private StudentAddressBook1Interface _sad;
        public StudentAddressBook1FacadeController(StudentAddressBook1Interface sad)
        {
            _sad = sad;
        }
       
        [Route("getinitialdata/{id:int}")]
        public Task<StudentAddressBook1DTO> getInitialData(int id)
        {
            return _sad.getInitailData(id);
        }

        [Route("classchange/{id:int}")]
        public Task<StudentAddressBook1DTO> classchange(int id)
        {
            return _sad.getInitailData(id);
        }

        [Route("yearchange")]
        public Task<StudentAddressBook1DTO> yearchange(StudentAddressBook1DTO id)
        {
            return _sad.yearchange(id);
        }

        [Route("sectinchange")]
        public Task<StudentAddressBook1DTO> sectionchange([FromBody] StudentAddressBook1DTO dto)
        {
            return  _sad.sectinchange(dto);
        }

        [HttpPost]
        [Route("getdetails")]
        //[Route("getenquirycontroller")]
        public async Task<StudentAddressBook1DTO> getdetails([FromBody] StudentAddressBook1DTO data)
        {           
            return await _sad.getdetails(data);
        }



        [Route("ExportToExcle/")]

        public string ExportToExcle([FromBody] StudentAddressBook1DTO reg)
        {
            string str = "";



            return str;
        }




    }
}
