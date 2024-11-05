using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class EmployeeServiceRecordReportDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<EmployeeServiceRecordReportDTO, EmployeeServiceRecordReportDTO> COMMM = new CommonDelegate<EmployeeServiceRecordReportDTO, EmployeeServiceRecordReportDTO>();

        public EmployeeServiceRecordReportDTO onloadgetdetails(EmployeeServiceRecordReportDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "EmployeeServiceRecordReportFacade/onloadgetdetails");
        }


        //getEmployeedetailsBySelection   

        public EmployeeServiceRecordReportDTO getEmployeedetailsBySelection(EmployeeServiceRecordReportDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "EmployeeServiceRecordReportFacade/getEmployeedetailsBySelection/");
        }

        public EmployeeServiceRecordReportDTO FilterEmployeeData(EmployeeServiceRecordReportDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "EmployeeServiceRecordReportFacade/FilterEmployeeData/");
        }

        public EmployeeServiceRecordReportDTO get_depts(EmployeeServiceRecordReportDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "EmployeeServiceRecordReportFacade/get_depts/");
        }
        public EmployeeServiceRecordReportDTO get_desig(EmployeeServiceRecordReportDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "EmployeeServiceRecordReportFacade/get_desig/");
        }
    }
}
