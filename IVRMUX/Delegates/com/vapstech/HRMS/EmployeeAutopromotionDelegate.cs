using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class EmployeeAutopromotionDelegate
    {

        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HR_Emp_AutopromotionDTO, HR_Emp_AutopromotionDTO> COMMM = new CommonDelegate<HR_Emp_AutopromotionDTO, HR_Emp_AutopromotionDTO>();

        public HR_Emp_AutopromotionDTO onloadgetdetails(HR_Emp_AutopromotionDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "EmployeeAutopromotionFacade/onloadgetdetails");
        }

        public HR_Emp_AutopromotionDTO savedetails(HR_Emp_AutopromotionDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "EmployeeAutopromotionFacade/");
        }
        public HR_Emp_AutopromotionDTO getRecorddetailsById(int id)
        {
            return COMMM.GetDataByIdHRMS(id, "EmployeeAutopromotionFacade/getRecordById/");
        }
        public HR_Emp_AutopromotionDTO deleterec(HR_Emp_AutopromotionDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "EmployeeAutopromotionFacade/deactivateRecordById/");
        }
        public HR_Emp_AutopromotionDTO getDetailsByEmployee(HR_Emp_AutopromotionDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "EmployeeAutopromotionFacade/getDetailsByEmployee/");
        }

    }
}
