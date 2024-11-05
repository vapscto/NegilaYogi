using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class MasterGroupTypeDelegate
    {
        private readonly object resource;
      //  private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HR_Master_GroupTypeDTO, HR_Master_GroupTypeDTO> COMMM = new CommonDelegate<HR_Master_GroupTypeDTO, HR_Master_GroupTypeDTO>();

        public HR_Master_GroupTypeDTO onloadgetdetails(HR_Master_GroupTypeDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "MasterGroupTypeFacade/onloadgetdetails");
        }

       public HR_Master_GroupTypeDTO Onchangedetails(HR_Master_GroupTypeDTO dto)
       {
         return COMMM.POSTDataHRMS(dto, "MasterGroupTypeFacade/Onchangedetails");
       }

    public HR_Master_GroupTypeDTO savedetails(HR_Master_GroupTypeDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "MasterGroupTypeFacade/");
        }
        public HR_Master_GroupTypeDTO getRecorddetailsById(int id)
        {
            return COMMM.GetDataByIdHRMS(id, "MasterGroupTypeFacade/getRecordById/");
        }
        public HR_Master_GroupTypeDTO deleterec(HR_Master_GroupTypeDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "MasterGroupTypeFacade/deactivateRecordById/");
        }


    }
}
