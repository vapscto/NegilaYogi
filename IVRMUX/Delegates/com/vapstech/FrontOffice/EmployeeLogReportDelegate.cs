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
    public class EmployeeLogReportDelegate
    {
        CommonDelegate<EmployeeLogReportDTO, EmployeeLogReportDTO> COMFRNT = new CommonDelegate<EmployeeLogReportDTO, EmployeeLogReportDTO>();
        public EmployeeLogReportDTO getdata(EmployeeLogReportDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "EmployeeLogReportFacade/getalldetails/");
        }
        public EmployeeLogReportDTO get_departments(EmployeeLogReportDTO student)
        {
            return COMFRNT.POSTDataHolidayReport(student, "EmployeeLogReportFacade/get_departments/");
        }
        public EmployeeLogReportDTO get_designation(EmployeeLogReportDTO student)
        {
            return COMFRNT.POSTDataHolidayReport(student, "EmployeeLogReportFacade/get_designation/");
        }
        public EmployeeLogReportDTO get_employee(EmployeeLogReportDTO student)
        {
            return COMFRNT.POSTDataHolidayReport(student, "EmployeeLogReportFacade/get_employee/");
        }
        public EmployeeLogReportDTO getrpt(EmployeeLogReportDTO student)
        {
            return COMFRNT.POSTDataHolidayReport(student, "EmployeeLogReportFacade/getrpt/");
        }
        public EmployeeLogReportDTO getsiglerpt(EmployeeLogReportDTO student)
        {
            return COMFRNT.POSTDataHolidayReport(student, "EmployeeLogReportFacade/getsiglerpt/");
        }
    }
}
