
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PortalHub.com.vaps.Employee.Interfaces;
using PreadmissionDTOs.com.vaps.Portals.Employee;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalHub.com.vaps.Employee.Controllers
{
    [Route("api/[controller]")]
    public class Employee_MedicalRecordFacade : Controller
    {

        // GET: api/values
        public Employee_MedicalRecordInterface _msg;
        public Employee_MedicalRecordFacade(Employee_MedicalRecordInterface msg)
        {
            _msg = msg;
        }
        // GET: api/values
        [Route("savedetail")]
        public Employee_MedicalRecordDTO savedetail([FromBody]Employee_MedicalRecordDTO data)
        {
            return _msg.savedetail(data);
        }
        [Route("Getdetails")]
        public Employee_MedicalRecordDTO Getdetails([FromBody]Employee_MedicalRecordDTO data)
        {
            return _msg.Getdetails(data);
        }

        [Route("deactivate")]
        public Employee_MedicalRecordDTO deactivated([FromBody]Employee_MedicalRecordDTO data)
        {
            return _msg.deactivate(data);
        }
        [Route("viewData")]
        public Employee_MedicalRecordDTO viewData([FromBody]Employee_MedicalRecordDTO data)
        {
            return _msg.viewData(data);
        }
        [Route("onclick_employee")]
        public Employee_MedicalRecordDTO onclick_employee([FromBody]Employee_MedicalRecordDTO data)
        {
            return _msg.onclick_employee(data);
        }

    }
}