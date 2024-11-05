using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class employeeSalaryaprovalflowDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<salaryApprovalFlowDTO, salaryApprovalFlowDTO> COMMM = new CommonDelegate<salaryApprovalFlowDTO, salaryApprovalFlowDTO>();

        public salaryApprovalFlowDTO onloadgetdetails(salaryApprovalFlowDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "salaryApprovalflowFacade/onloadgetdetails");
        }

        //FilterEmployeeData
        public salaryApprovalFlowDTO FilterEmployeeData(salaryApprovalFlowDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "salaryApprovalflowFacade/FilterEmployeeData/");
        }

        //getEmployeedetailsBySelection   

        public salaryApprovalFlowDTO getEmployeedetailsBySelection(salaryApprovalFlowDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "salaryApprovalflowFacade/getEmployeedetailsBySelection/");
        }

        public salaryApprovalFlowDTO get_depts(salaryApprovalFlowDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "salaryApprovalflowFacade/get_depts/");
        }
        public salaryApprovalFlowDTO get_desig(salaryApprovalFlowDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "salaryApprovalflowFacade/get_desig/");
        }
    }
}
