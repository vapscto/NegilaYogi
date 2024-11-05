using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HRMSServicesHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.HRMS;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HRMSServicesHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class EmployeeAutopromotionFacadeController : Controller
    {
        public EmployeeAutopromotionInterface _ads;

        public EmployeeAutopromotionFacadeController(EmployeeAutopromotionInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public HR_Emp_AutopromotionDTO getinitialdata([FromBody]HR_Emp_AutopromotionDTO dto)
        {
            return _ads.getBasicData(dto);
        }

        // POST api/values
        [HttpPost]
        public HR_Emp_AutopromotionDTO Post([FromBody]HR_Emp_AutopromotionDTO dto)
        {
            return _ads.SaveUpdate(dto);
        }

        [Route("getRecordById/{id:int}")]

        public HR_Emp_AutopromotionDTO getcatgrydet(int id)
        {
            // id = 12;
            return _ads.editData(id);
        }
        [Route("deactivateRecordById")]
        public HR_Emp_AutopromotionDTO deactivateRecordById([FromBody]HR_Emp_AutopromotionDTO dto)
        {
            return _ads.deactivate(dto);
        }

        [Route("getDetailsByEmployee")]
        public HR_Emp_AutopromotionDTO getDetailsByEmployee([FromBody]HR_Emp_AutopromotionDTO dto)
        {
            return _ads.getDetailsByEmployee(dto);
        }

    }
}
