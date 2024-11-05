using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace corewebapi18072016.Delegates.com.vapstech.FrontOffice
{
    public class EmployeeInOutReportDelegate
    {
        CommonDelegate<EmployeeInOutReportDTO, EmployeeInOutReportDTO> COMFRNT = new CommonDelegate<EmployeeInOutReportDTO, EmployeeInOutReportDTO>();
        public EmployeeInOutReportDTO getdata(EmployeeInOutReportDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "EmployeeInOutReportFacade/getalldetails/");
        }
        public EmployeeInOutReportDTO get_departments(EmployeeInOutReportDTO student)
        {
            return COMFRNT.POSTDataHolidayReport(student, "EmployeeInOutReportFacade/get_departments/");
        }
        public EmployeeInOutReportDTO get_designation(EmployeeInOutReportDTO student)
        {
            return COMFRNT.POSTDataHolidayReport(student, "EmployeeInOutReportFacade/get_designation/");

        }
        public EmployeeInOutReportDTO get_employee(EmployeeInOutReportDTO student)
        {

            return COMFRNT.POSTDataHolidayReport(student, "EmployeeInOutReportFacade/get_employee/");
        }
        public EmployeeInOutReportDTO getrpt(EmployeeInOutReportDTO student)
        {
            return COMFRNT.POSTDataHolidayReport(student, "EmployeeInOutReportFacade/getrpt/");
        }
    }
}
