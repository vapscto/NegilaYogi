using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
   
    public class MasterAllowanceDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<MasterAllowanceDTO, MasterAllowanceDTO> COMMM = new CommonDelegate<MasterAllowanceDTO, MasterAllowanceDTO>();

        public MasterAllowanceDTO onloadgetdetails(MasterAllowanceDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "MasterAllowanceFacade/onloadgetdetails");
        }

        public MasterAllowanceDTO savedetails(MasterAllowanceDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "MasterAllowanceFacade/");
        }
        public MasterAllowanceDTO getRecorddetailsById(int id)
        {
            return COMMM.GetDataByIdHRMS(id, "MasterAllowanceFacade/getRecordById/");
        }
        public MasterAllowanceDTO deleterec(MasterAllowanceDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "MasterAllowanceFacade/deactivateRecordById/");
        }

    }
}
