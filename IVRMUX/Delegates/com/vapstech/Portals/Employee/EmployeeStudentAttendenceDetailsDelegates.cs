using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.Portals.Employee;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Delegates.com.vapstech.Portals.Employee
{
    [Route("api/[controller]")]
    public class EmployeeStudentAttendenceDetailsDelegates : Controller
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<EmployeeDashboardDTO, EmployeeDashboardDTO> COMMM = new CommonDelegate<EmployeeDashboardDTO, EmployeeDashboardDTO>();
        public EmployeeDashboardDTO Getdetails(EmployeeDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "EmployeeStudentAttendenceDetailsFacade/Getdetails/");
        }

        public EmployeeDashboardDTO getclass(EmployeeDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "EmployeeStudentAttendenceDetailsFacade/getclass/");
        }
        public EmployeeDashboardDTO Getsection(EmployeeDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "EmployeeStudentAttendenceDetailsFacade/Getsection/");
        }
        public EmployeeDashboardDTO GetAttendence(EmployeeDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "EmployeeStudentAttendenceDetailsFacade/GetAttendence/");
        }

        public EmployeeDashboardDTO GetIndividualAttendence(EmployeeDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "EmployeeStudentAttendenceDetailsFacade/GetIndividualAttendence/");
        }


    }
}
