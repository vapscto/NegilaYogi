﻿using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Portals.Employee
{
    public class EmployeeStudentDetailsDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<EmployeeDashboardDTO, EmployeeDashboardDTO> COMMM = new CommonDelegate<EmployeeDashboardDTO, EmployeeDashboardDTO>();

        public EmployeeDashboardDTO getalldetails(EmployeeDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "EmployeeStudentDetailsFacade/getdata/");
        }
      
        public EmployeeDashboardDTO get_class(EmployeeDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "EmployeeStudentDetailsFacade/get_class/");
        }
        public EmployeeDashboardDTO get_section(EmployeeDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "EmployeeStudentDetailsFacade/get_section/");
        }
        public EmployeeDashboardDTO get_student(EmployeeDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "EmployeeStudentDetailsFacade/get_student/");
        }
    }
}