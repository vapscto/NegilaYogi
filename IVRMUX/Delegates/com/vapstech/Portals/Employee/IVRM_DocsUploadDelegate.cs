using CommonLibrary;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Portals.Employee
{
    public class IVRM_DocsUploadDelegate
    {
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<IVRM_DocsUploadDTO, IVRM_DocsUploadDTO> COMMM = new CommonDelegate<IVRM_DocsUploadDTO, IVRM_DocsUploadDTO>();

        public IVRM_DocsUploadDTO Getdetails(IVRM_DocsUploadDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_DocsUploadFacade/Getdetails/");
        }

        public IVRM_DocsUploadDTO savedetail(IVRM_DocsUploadDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_DocsUploadFacade/savedetail/");
        }
       
        public IVRM_DocsUploadDTO get_classes(IVRM_DocsUploadDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_DocsUploadFacade/get_classes/");
        }

        public IVRM_DocsUploadDTO getsectiondata(IVRM_DocsUploadDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_DocsUploadFacade/getsectiondata/");
        }

        public IVRM_DocsUploadDTO editData(IVRM_DocsUploadDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_DocsUploadFacade/editData/");
        }

        public IVRM_DocsUploadDTO deactivate(IVRM_DocsUploadDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_DocsUploadFacade/deactivate/");
        }

    }
}
