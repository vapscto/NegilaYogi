using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.Portals.Student;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.Portals.IVRM;

namespace corewebapi18072016.Delegates.com.vapstech.Portals.Student
{
    public class IVRM_InteractionsDelegate
    {

        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<IVRM_School_InteractionsDTO, IVRM_School_InteractionsDTO> COMMM = new CommonDelegate<IVRM_School_InteractionsDTO, IVRM_School_InteractionsDTO>();
        public IVRM_School_InteractionsDTO getloaddata(IVRM_School_InteractionsDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_InteractionsFacade/getloaddata/");
        }
        public IVRM_School_InteractionsDTO getdetails(IVRM_School_InteractionsDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_InteractionsFacade/getdetails/");
        }    
        public IVRM_School_InteractionsDTO getstudent(IVRM_School_InteractionsDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_InteractionsFacade/getstudent/");
        }
        public IVRM_School_InteractionsDTO savedetails(IVRM_School_InteractionsDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_InteractionsFacade/savedetails/");
        }
     

        public IVRM_School_InteractionsDTO savereply(IVRM_School_InteractionsDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_InteractionsFacade/savereply/");
        }
         public IVRM_School_InteractionsDTO deletemsg(IVRM_School_InteractionsDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_InteractionsFacade/deletemsg/");
        }
         public IVRM_School_InteractionsDTO deleteinboxmsg(IVRM_School_InteractionsDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_InteractionsFacade/deleteinboxmsg/");
        }


        public IVRM_School_InteractionsDTO reply(IVRM_School_InteractionsDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_InteractionsFacade/reply/");
        }
        public IVRM_School_InteractionsDTO seen(IVRM_School_InteractionsDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_InteractionsFacade/seen/");
        }

    }
}
