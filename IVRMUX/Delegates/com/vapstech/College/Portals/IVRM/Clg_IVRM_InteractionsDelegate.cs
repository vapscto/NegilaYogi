using CommonLibrary;
using PreadmissionDTOs.com.vaps.Portals.IVRM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Portals.IVRM
{
    public class Clg_IVRM_InteractionsDelegate
    {


        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<IVRM_School_InteractionsDTO, IVRM_School_InteractionsDTO> COMMM = new CommonDelegate<IVRM_School_InteractionsDTO, IVRM_School_InteractionsDTO>();
        public IVRM_School_InteractionsDTO getloaddata(IVRM_School_InteractionsDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "Clg_IVRM_InteractionsFacade/getloaddata/");
        }
        public IVRM_School_InteractionsDTO getdetails(IVRM_School_InteractionsDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "Clg_IVRM_InteractionsFacade/getdetails/");
        }
        public IVRM_School_InteractionsDTO getstudent(IVRM_School_InteractionsDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "Clg_IVRM_InteractionsFacade/getstudent/");
        }
        public IVRM_School_InteractionsDTO Getbranch(IVRM_School_InteractionsDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "Clg_IVRM_InteractionsFacade/Getbranch/");
        }
        public IVRM_School_InteractionsDTO Getsection(IVRM_School_InteractionsDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "Clg_IVRM_InteractionsFacade/Getsection/");
        }
        public IVRM_School_InteractionsDTO Getsemester(IVRM_School_InteractionsDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "Clg_IVRM_InteractionsFacade/Getsemester/");
        }
        public IVRM_School_InteractionsDTO savedetails(IVRM_School_InteractionsDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "Clg_IVRM_InteractionsFacade/savedetails/");
        }


        public IVRM_School_InteractionsDTO savereply(IVRM_School_InteractionsDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "Clg_IVRM_InteractionsFacade/savereply/");
        }


        public IVRM_School_InteractionsDTO reply(IVRM_School_InteractionsDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "Clg_IVRM_InteractionsFacade/reply/");
        }
         public IVRM_School_InteractionsDTO deletemsg(IVRM_School_InteractionsDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "Clg_IVRM_InteractionsFacade/deletemsg/");
        }
         public IVRM_School_InteractionsDTO deleteinboxmsg(IVRM_School_InteractionsDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "Clg_IVRM_InteractionsFacade/deleteinboxmsg/");
        }

    }
}
