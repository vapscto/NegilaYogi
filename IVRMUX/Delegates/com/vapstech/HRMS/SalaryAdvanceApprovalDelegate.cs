using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class SalaryAdvanceApprovalDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HR_Emp_SalaryAdvanceDTO, HR_Emp_SalaryAdvanceDTO> COMMM = new CommonDelegate<HR_Emp_SalaryAdvanceDTO, HR_Emp_SalaryAdvanceDTO>();

        public HR_Emp_SalaryAdvanceDTO onloadgetdetails(HR_Emp_SalaryAdvanceDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "HREmpSalaryAdvanceFacade/onloadgetdetails");
        }

    }
}
