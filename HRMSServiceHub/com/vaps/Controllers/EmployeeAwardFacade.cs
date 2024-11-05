using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMSServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HRMSServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class EmployeeAwardFacade : Controller
    {
        public EmployeeAwardInterface _Interface;

        public EmployeeAwardFacade(EmployeeAwardInterface parameter)
        {
            _Interface = parameter;
        }

        [Route("getalldetails")]
        public HR_Employee_Awards_DTO getalldetails([FromBody] HR_Employee_Awards_DTO data)
        {
            return _Interface.getalldetails(data);
        }

        [Route("get_depchange")]
        public HR_Employee_Awards_DTO get_depchange([FromBody] HR_Employee_Awards_DTO data)
        {
            return _Interface.get_depchange(data);
        }

        [Route("get_employee")]
        public HR_Employee_Awards_DTO get_employee([FromBody] HR_Employee_Awards_DTO data)
        {
            return _Interface.get_employee(data);
        }
        [Route("saverecord")]
        public HR_Employee_Awards_DTO saverecord([FromBody] HR_Employee_Awards_DTO data)
        {
            return _Interface.saverecord(data);
        }

        [Route("editrecord")]
        public HR_Employee_Awards_DTO editrecord([FromBody] HR_Employee_Awards_DTO data)
        {
            return _Interface.editrecord(data);
        }
        [Route("deactive")]
        public HR_Employee_Awards_DTO deactive([FromBody] HR_Employee_Awards_DTO data)
        {
            return _Interface.deactive(data);
        }

       

    }
}
