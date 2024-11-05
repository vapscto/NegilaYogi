using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Employee.Interfaces
{
    public interface EmployeeStudentSearchInterface
    {
        EmployeeDashboardDTO getdatastuacadgrp(EmployeeDashboardDTO data);
        EmployeeDashboardDTO getstudentdetails(EmployeeDashboardDTO data);
    }
}