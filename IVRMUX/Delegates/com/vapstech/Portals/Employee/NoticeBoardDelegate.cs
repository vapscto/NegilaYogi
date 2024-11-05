using CommonLibrary;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Portals.Employee
{
    public class NoticeBoardDelegats
    {
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<IVRM_NoticeBoardDTO, IVRM_NoticeBoardDTO> COMMM = new CommonDelegate<IVRM_NoticeBoardDTO, IVRM_NoticeBoardDTO>();
        public IVRM_NoticeBoardDTO savedetail(IVRM_NoticeBoardDTO data)
        {
            return COMMM.POSTPORTALData(data, "NoticeBoardFacade/savedetail/");
        }
        public IVRM_NoticeBoardDTO Getdetails(IVRM_NoticeBoardDTO data)
        {
            return COMMM.POSTPORTALData(data, "NoticeBoardFacade/Getdetails/");
        }
        public IVRM_NoticeBoardDTO deactivate(IVRM_NoticeBoardDTO data)
        {
            return COMMM.POSTPORTALData(data, "NoticeBoardFacade/deactivate/");
        }

        public IVRM_NoticeBoardDTO editdetails(IVRM_NoticeBoardDTO data)
        {
            return COMMM.POSTPORTALData(data, "NoticeBoardFacade/editdetails/");
        }

          public IVRM_NoticeBoardDTO get_noticelist(IVRM_NoticeBoardDTO data)
        {
            return COMMM.POSTPORTALData(data, "NoticeBoardFacade/get_noticelist/");
        }
         public IVRM_NoticeBoardDTO getsection(IVRM_NoticeBoardDTO data)
        {
            return COMMM.POSTPORTALData(data, "NoticeBoardFacade/getsection/");
        }
         public IVRM_NoticeBoardDTO getstudent(IVRM_NoticeBoardDTO data)
        {
            return COMMM.POSTPORTALData(data, "NoticeBoardFacade/getstudent/");
        }
          public IVRM_NoticeBoardDTO Deptselectiondetails(IVRM_NoticeBoardDTO data)
        {
            return COMMM.POSTPORTALData(data, "NoticeBoardFacade/Deptselectiondetails/");
        }
        public IVRM_NoticeBoardDTO Desgselectiondetails(IVRM_NoticeBoardDTO data)
        {
            return COMMM.POSTPORTALData(data, "NoticeBoardFacade/Desgselectiondetails/");
        }
        public IVRM_NoticeBoardDTO viewData(IVRM_NoticeBoardDTO data)
        {
            return COMMM.POSTPORTALData(data, "NoticeBoardFacade/viewData/");
        }
        public IVRM_NoticeBoardDTO viewrecords(IVRM_NoticeBoardDTO data)
        {
            return COMMM.POSTPORTALData(data, "NoticeBoardFacade/viewrecords/");
        }


    }
}
