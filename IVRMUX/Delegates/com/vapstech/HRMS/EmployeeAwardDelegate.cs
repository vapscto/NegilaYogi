using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.HRMS
{
    public class EmployeeAwardDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HR_Employee_Awards_DTO, HR_Employee_Awards_DTO> COMMM = new CommonDelegate<HR_Employee_Awards_DTO, HR_Employee_Awards_DTO>();
        public HR_Employee_Awards_DTO getalldetails(HR_Employee_Awards_DTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "EmployeeAwardFacade/getalldetails");
        }

        public HR_Employee_Awards_DTO get_depchange(HR_Employee_Awards_DTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "EmployeeAwardFacade/get_depchange");
        }

        public HR_Employee_Awards_DTO get_employee(HR_Employee_Awards_DTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "EmployeeAwardFacade/get_employee");
        }

        public HR_Employee_Awards_DTO saverecord(HR_Employee_Awards_DTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "EmployeeAwardFacade/saverecord");
        }

        public HR_Employee_Awards_DTO editrecord(HR_Employee_Awards_DTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "EmployeeAwardFacade/editrecord");
        }
        public HR_Employee_Awards_DTO deactive(HR_Employee_Awards_DTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "EmployeeAwardFacade/deactive");
        }
        public HR_Employee_Awards_DTO viewuploadflies(HR_Employee_Awards_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "EmployeeAwardFacade/viewuploadflies");
        }
        public HR_Employee_Awards_DTO deleteuploadfile(HR_Employee_Awards_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "EmployeeAwardFacade/deleteuploadfile");
        }

    }
}
