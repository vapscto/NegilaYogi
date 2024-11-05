using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.VMS.HRMS
{
    public class masterPositionDelegates
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HR_Master_PositionDTO, HR_Master_PositionDTO> COMMM = new CommonDelegate<HR_Master_PositionDTO, HR_Master_PositionDTO>();

        public HR_Master_PositionDTO onloadgetdetails(HR_Master_PositionDTO dto)
        {
            return COMMM.POSTVMS(dto, "masterPositionFacade/onloadgetdetails");
        }

        public HR_Master_PositionDTO savedetails(HR_Master_PositionDTO maspage)
        {
            return COMMM.POSTVMS(maspage, "masterPositionFacade/");
        }
        public HR_Master_PositionDTO getdata(HR_Master_PositionDTO maspage)
        {
            return COMMM.POSTVMS(maspage, "masterPositionFacade/getdata");
        }
        public HR_Master_PositionDTO getRecorddetailsById(int id)
        {
            return COMMM.GetVMS(id, "masterPositionFacade/getRecordById/");
        }
        public HR_Master_PositionDTO deleterec(HR_Master_PositionDTO maspage)
        {
            return COMMM.POSTVMS(maspage, "masterPositionFacade/deactivateRecordById/");
        }


    }
}
