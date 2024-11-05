using CommonLibrary;
using PreadmissionDTOs.com.vaps.LeaveManagement;
using System;


namespace IVRMUX.Delegates.com.vapstech.LeaveManagement
{
    public class EmployeeOnDutyReportDeleget
    {

        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<EmployeeOnDutyReportDTO, EmployeeOnDutyReportDTO> COMMM = new CommonDelegate<EmployeeOnDutyReportDTO, EmployeeOnDutyReportDTO>();

        public EmployeeOnDutyReportDTO getalldetails(EmployeeOnDutyReportDTO dto)
        {
            return COMMM.POSTDataOnlineLeave(dto, "EmployeeOnDutyReportFacade/getalldetails/");
        }
        public EmployeeOnDutyReportDTO getEmployeedetailsBySelection(EmployeeOnDutyReportDTO maspage)
        {
            return COMMM.POSTDataOnlineLeave(maspage, "EmployeeOnDutyReportFacade/getEmployeedetailsBySelection/");
        }
    }
}
