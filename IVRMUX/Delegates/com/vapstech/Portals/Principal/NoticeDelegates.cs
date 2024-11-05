using CommonLibrary;
using PreadmissionDTOs.com.vaps.Portals.Principal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Portals.Principal
{
    public class NoticeDelegates
    {
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<Notice_DTO, Notice_DTO> COMMM = new CommonDelegate<Notice_DTO, Notice_DTO>();
        public Notice_DTO savedetail(Notice_DTO data)
        {
            return COMMM.POSTPORTALData(data, "NoticeFacade/savedetail/");
        }
        public Notice_DTO Getdetails(Notice_DTO data)
        {
            return COMMM.POSTPORTALData(data, "NoticeFacade/Getdetails/");
        }
        public Notice_DTO deactivate(Notice_DTO data)
        {
            return COMMM.POSTPORTALData(data, "NoticeFacade/deactivate/");
        }

    }
}
