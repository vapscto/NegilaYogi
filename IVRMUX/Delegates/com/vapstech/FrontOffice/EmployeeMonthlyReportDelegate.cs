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
    public class EmployeeMonthlyReportDelegate
    {
        CommonDelegate<EmployeeMonthlyReportDTO, EmployeeMonthlyReportDTO> COMFRNT = new CommonDelegate<EmployeeMonthlyReportDTO, EmployeeMonthlyReportDTO>();
        public EmployeeMonthlyReportDTO getdata(EmployeeMonthlyReportDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "EmployeeMonthlyReportFacade/getalldetails/");
        }
        public EmployeeMonthlyReportDTO get_departments(EmployeeMonthlyReportDTO student)
        {
            return COMFRNT.POSTDataHolidayReport(student, "EmployeeMonthlyReportFacade/get_departments/");
        }
        public EmployeeMonthlyReportDTO get_designation(EmployeeMonthlyReportDTO student)
        {
            return COMFRNT.POSTDataHolidayReport(student, "EmployeeYearlyReportFacade/get_designation/");
        }
        public EmployeeMonthlyReportDTO get_employee(EmployeeMonthlyReportDTO student)
        {
            return COMFRNT.POSTDataHolidayReport(student, "EmployeeMonthlyReportFacade/get_employee/");
        }
        public EmployeeMonthlyReportDTO getrpt(EmployeeMonthlyReportDTO student)
        {
            return COMFRNT.POSTDataHolidayReport(student, "EmployeeMonthlyReportFacade/getrpt/");
        }
        public EmployeeMonthlyReportDTO getOTrpt(EmployeeMonthlyReportDTO student)
        {
            return COMFRNT.POSTDataHolidayReport(student, "EmployeeMonthlyReportFacade/getOTrpt/");
        }
        public EmployeeMonthlyReportDTO getrptStJames(EmployeeMonthlyReportDTO student)
        {
            return COMFRNT.POSTDataHolidayReport(student, "EmployeeMonthlyReportFacade/getrptStJames/");
        }        
    }
}
