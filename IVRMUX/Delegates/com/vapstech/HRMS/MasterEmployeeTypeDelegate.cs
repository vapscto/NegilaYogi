using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class MasterEmployeeTypeDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HR_Master_EmployeeTypeDTO, HR_Master_EmployeeTypeDTO> COMMM = new CommonDelegate<HR_Master_EmployeeTypeDTO, HR_Master_EmployeeTypeDTO>();

        public HR_Master_EmployeeTypeDTO onloadgetdetails(HR_Master_EmployeeTypeDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "MasterEmployeeTypeFacade/onloadgetdetails");
        }

        public HR_Master_EmployeeTypeDTO savedetails(HR_Master_EmployeeTypeDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "MasterEmployeeTypeFacade/");
        }
        public HR_Master_EmployeeTypeDTO getRecorddetailsById(int id)
        {
            return COMMM.GetDataByIdHRMS(id, "MasterEmployeeTypeFacade/getRecordById/");
        }
        public HR_Master_EmployeeTypeDTO deleterec(HR_Master_EmployeeTypeDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "MasterEmployeeTypeFacade/deactivateRecordById/");
        }

       

       

    }
}
