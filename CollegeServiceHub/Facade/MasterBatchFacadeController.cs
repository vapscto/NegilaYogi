using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;
using CollegeServiceHub.Interface;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeServiceHub.Facade
{
    [Route("api/[controller]")]
    public class MasterBatchFacadeController : Controller
    {
        MasterBatchInterface _batchint;
        public MasterBatchFacadeController(MasterBatchInterface batchint)
        {
            _batchint = batchint;
        }
        [Route("savebatch")]
        public AdmCollegeMasterBatchDTO savebatch([FromBody] AdmCollegeMasterBatchDTO data)
        {
            return _batchint.savebatch(data);
        }
        [Route("editbatch")]
        public AdmCollegeMasterBatchDTO editbatch([FromBody] AdmCollegeMasterBatchDTO data)
        {
            return _batchint.editbatch(data);
        }
        [Route("getdata")]
        public AdmCollegeMasterBatchDTO getdata([FromBody] AdmCollegeMasterBatchDTO data)
        {
            return _batchint.getdata(data);
        }
        [Route("activedeactivebatch")]
        public AdmCollegeMasterBatchDTO activedeactivebatch([FromBody] AdmCollegeMasterBatchDTO data)
        {
            return _batchint.activedeactivebatch(data);
        }
    }
}
