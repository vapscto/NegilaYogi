using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class MasterDepartmentDelegate
    {
        private readonly object resource;
      //  private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HR_Master_DepartmentDTO, HR_Master_DepartmentDTO> COMMM = new CommonDelegate<HR_Master_DepartmentDTO, HR_Master_DepartmentDTO>();

        public HR_Master_DepartmentDTO onloadgetdetails(HR_Master_DepartmentDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "MasterDepartmentFacade/onloadgetdetails");
        }

        public HR_Master_DepartmentDTO Onchangedetails(HR_Master_DepartmentDTO dto)
       {
           return COMMM.POSTDataHRMS(dto, "MasterDepartmentFacade/Onchangedetails");
       }

    public HR_Master_DepartmentDTO savedetails(HR_Master_DepartmentDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "MasterDepartmentFacade/");
        }
        public HR_Master_DepartmentDTO getRecorddetailsById(int id)
        {
            return COMMM.GetDataByIdHRMS(id, "MasterDepartmentFacade/getRecordById/");
        }
        public HR_Master_DepartmentDTO deleterec(HR_Master_DepartmentDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "MasterDepartmentFacade/deactivateRecordById/");
        }
    }
}
