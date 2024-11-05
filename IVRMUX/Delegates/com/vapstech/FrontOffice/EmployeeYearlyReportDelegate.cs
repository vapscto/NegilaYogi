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
    public class EmployeeYearlyReportDelegate
    {
        CommonDelegate<EmployeeYearlyReportDTO, EmployeeYearlyReportDTO> COMFRNT = new CommonDelegate<EmployeeYearlyReportDTO, EmployeeYearlyReportDTO>();


        public EmployeeYearlyReportDTO getdata(EmployeeYearlyReportDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "EmployeeYearlyReportFacade/getalldetails/");
        }
        public EmployeeYearlyReportDTO get_departments(EmployeeYearlyReportDTO student)
        {
            return COMFRNT.POSTDataHolidayReport(student, "EmployeeYearlyReportFacade/get_departments/");
        }
        public EmployeeYearlyReportDTO get_designation(EmployeeYearlyReportDTO student)
        {
            return COMFRNT.POSTDataHolidayReport(student, "EmployeeYearlyReportFacade/get_designation/");
        }
        public EmployeeYearlyReportDTO get_employee(EmployeeYearlyReportDTO student)
        {
            return COMFRNT.POSTDataHolidayReport(student, "EmployeeYearlyReportFacade/get_employee/");
        }
        public EmployeeYearlyReportDTO getrpt(EmployeeYearlyReportDTO student)
        {
            return COMFRNT.POSTDataHolidayReport(student, "EmployeeYearlyReportFacade/getrpt/");
        }
    }
}
