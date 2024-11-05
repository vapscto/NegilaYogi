using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AlumniHub.Com.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Alumni;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AlumniHub.Com.Facade
{
    [Route("api/[controller]")]
    public class SchoolALUDASHFacade : Controller
    {
        SchoolALUDASHInterface _aluint;
        public SchoolALUDASHFacade(SchoolALUDASHInterface stu)
        {
            _aluint = stu;
        }

        [HttpPost]
        [Route("getloaddata")]
        public AlumniStudentDTO getdataa([FromBody]AlumniStudentDTO value)
        {
            return _aluint.getloaddata(value);
        }

        [Route("yearwiselist")]
        public AlumniStudentDTO yearwiselist([FromBody] AlumniStudentDTO data)
        {
            return _aluint.yearwiselist(data);
        }
        [Route("classwisestudent")]
        public AlumniStudentDTO classwisestudent([FromBody] AlumniStudentDTO data)
        {
            return _aluint.classwisestudent(data);
        }
        [Route("AluminiBirthday")]
        public AlumniStudentDTO AluminiBirthday([FromBody] AlumniStudentDTO data)
        {
            return _aluint.AluminiBirthday(data);
        }

        [Route("getgallery")]
        public AlumniStudentDTO getgallery([FromBody] AlumniStudentDTO data)
        {
            return _aluint.getgallery(data);
        }
        [Route("viewgallery")]
        public AlumniStudentDTO viewgallery([FromBody] AlumniStudentDTO data)
        {
            return _aluint.viewgallery(data);
        }
         [Route("alumninotice")]
        public AlumniStudentDTO alumninotice([FromBody] AlumniStudentDTO data)
        {
            return _aluint.alumninotice(data);
        }
         [Route("viewnotice")]
        public AlumniStudentDTO viewnotice([FromBody] AlumniStudentDTO data)
        {
            return _aluint.viewnotice(data);
        }

    }
}
