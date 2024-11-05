using CommonLibrary;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Portals.Employee
{
    public class EmployeePunchAttendenceDelegate
    {
        CommonDelegate<EmployeeDashboardDTO, EmployeeDashboardDTO> COMFRNT = new CommonDelegate<EmployeeDashboardDTO, EmployeeDashboardDTO>();
        public EmployeeDashboardDTO getdata(EmployeeDashboardDTO data)
        {
            return COMFRNT.POSTPORTALData(data, "EmployeePunchAttendenceFacade/getalldetails/");
        }
       
        public EmployeeDashboardDTO getrpt(EmployeeDashboardDTO student)
        {
            return COMFRNT.POSTPORTALData(student, "EmployeePunchAttendenceFacade/getrpt/");
        }
    }
}
