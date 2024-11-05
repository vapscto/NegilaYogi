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
    public class StudentTcReportFacadeController : Controller
    {

        public StudentTcReportInterface _feegrouppagee;
        // GET: api/values

        public StudentTcReportFacadeController(StudentTcReportInterface maspag)
        {
            _feegrouppagee = maspag;
        }
       // [HttpGet]
       


        [Route("getdetails/{id:int}")]
        public StudentTcReportDTO getorgdet(int id)
       
        {
            StudentTcReportDTO data = new StudentTcReportDTO();
            data.mid = id;
            return _feegrouppagee.getdetails(data);
        }
      

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }


        [Route("GetData")]
        public Task<StudentTcReportDTO> GetData([FromBody]StudentTcReportDTO dto)
        {
            return _feegrouppagee.Getdata(dto);
        }

        [Route("getclass")]
        public StudentTcReportDTO getclass([FromBody]StudentTcReportDTO dto)
        {
            return _feegrouppagee.getclass(dto);
        }
        
      [Route("getsecton")]
        public StudentTcReportDTO getsecton([FromBody]StudentTcReportDTO dto)
        {
            return _feegrouppagee.getsecton(dto);
        }
        
        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
