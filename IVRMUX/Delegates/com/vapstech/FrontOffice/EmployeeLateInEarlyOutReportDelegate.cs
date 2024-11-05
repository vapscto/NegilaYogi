using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.FrontOffice
{
    public class EmployeeLateInEarlyOutReportDelegate
    {
        CommonDelegate<EmployeeLateInEarlyOutReportDTO, EmployeeLateInEarlyOutReportDTO> COMFRNT = new CommonDelegate<EmployeeLateInEarlyOutReportDTO, EmployeeLateInEarlyOutReportDTO>();
        public EmployeeLateInEarlyOutReportDTO getdata(EmployeeLateInEarlyOutReportDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "EmployeeLateInEarlyOutReportFacade/getalldetails/");
        }
        public EmployeeLateInEarlyOutReportDTO get_departments(EmployeeLateInEarlyOutReportDTO student)
        {
            return COMFRNT.POSTDataHolidayReport(student, "EmployeeLateInEarlyOutReportFacade/get_departments/");
        }
        public EmployeeLateInEarlyOutReportDTO get_designation(EmployeeLateInEarlyOutReportDTO student)
        {
            return COMFRNT.POSTDataHolidayReport(student, "EmployeeLateInEarlyOutReportFacade/get_designation/");
        }
        public EmployeeLateInEarlyOutReportDTO get_employee(EmployeeLateInEarlyOutReportDTO student)
        {
            return COMFRNT.POSTDataHolidayReport(student, "EmployeeLateInEarlyOutReportFacade/get_employee/");
        }
        public EmployeeLateInEarlyOutReportDTO getrpt(EmployeeLateInEarlyOutReportDTO student)
        {
            return COMFRNT.POSTDataHolidayReport(student, "EmployeeLateInEarlyOutReportFacade/getrpt/");
        }
    }
}
