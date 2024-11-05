using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.VMS.HRMS
{
    public class masterPositionTypeDelegates
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HR_Master_PostionTypeDTO, HR_Master_PostionTypeDTO> COMMM = new CommonDelegate<HR_Master_PostionTypeDTO, HR_Master_PostionTypeDTO>();

        public HR_Master_PostionTypeDTO onloadgetdetails(HR_Master_PostionTypeDTO dto)
        {
            return COMMM.POSTVMS(dto, "masterPositionTypeFacade/onloadgetdetails");
        }

        public HR_Master_PostionTypeDTO savedetails(HR_Master_PostionTypeDTO maspage)
        {
            return COMMM.POSTVMS(maspage, "masterPositionTypeFacade/");
        }
        public HR_Master_PostionTypeDTO getdata(HR_Master_PostionTypeDTO maspage)
        {
            return COMMM.POSTVMS(maspage, "masterPositionTypeFacade/getdata");
        }
        public HR_Master_PostionTypeDTO getRecorddetailsById(int id)
        {
            return COMMM.GetVMS(id, "masterPositionTypeFacade/getRecordById/");
        }
        public HR_Master_PostionTypeDTO deleterec(HR_Master_PostionTypeDTO maspage)
        {
            return COMMM.POSTVMS(maspage, "masterPositionTypeFacade/deactivateRecordById/");
        }


    }
}
