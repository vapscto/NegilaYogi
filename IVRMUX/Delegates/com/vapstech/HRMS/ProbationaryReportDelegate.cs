using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.HRMS
{
    public class ProbationaryReportDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<EmployeeProfileReportDTO, EmployeeProfileReportDTO> COMMM = new CommonDelegate<EmployeeProfileReportDTO, EmployeeProfileReportDTO>();
        public EmployeeProfileReportDTO getalldetails(EmployeeProfileReportDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "ProbationaryReportFacade/getalldetails");
        }
        public EmployeeProfileReportDTO getProbationaryReport(EmployeeProfileReportDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "ProbationaryReportFacade/getProbationaryReport/");
        }
        public EmployeeProfileReportDTO get_departments(EmployeeProfileReportDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "ProbationaryReportFacade/get_departments/");
        }
        public EmployeeProfileReportDTO get_designation(EmployeeProfileReportDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "ProbationaryReportFacade/get_designation/");
        }
    }
}
