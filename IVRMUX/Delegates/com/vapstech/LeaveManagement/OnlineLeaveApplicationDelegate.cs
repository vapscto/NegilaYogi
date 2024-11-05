using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.LeaveManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.LeaveManagement
{
    public class OnlineLeaveApplicationDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<LeaveCreditDTO, LeaveCreditDTO> COMMM = new CommonDelegate<LeaveCreditDTO, LeaveCreditDTO>();

   
        public LeaveCreditDTO savedata(LeaveCreditDTO data)
        {
            return COMMM.POSTDataOnlineLeave(data, "OnlineLeaveApplicationFacade/save/");
        }
        
        public LeaveCreditDTO saveadminLeave(LeaveCreditDTO data)
        {
            return COMMM.POSTDataOnlineLeave(data, "OnlineLeaveApplicationFacade/saveadminLeave/");
        }

        public LeaveCreditDTO getonlineLeave(LeaveCreditDTO data)
        {
            return COMMM.POSTDataOnlineLeave(data, "OnlineLeaveApplicationFacade/getonlineLeave/");
        }
        public LeaveCreditDTO getonlineLeavestatus(LeaveCreditDTO data)
        {
            return COMMM.POSTDataOnlineLeave(data, "OnlineLeaveApplicationFacade/getonlineLeavestatus/");
        }

        public LeaveCreditDTO getemployeeadmin(LeaveCreditDTO data)
        {
            return COMMM.POSTDataOnlineLeave(data, "OnlineLeaveApplicationFacade/getemployeeadmin/");
        }

        public LeaveCreditDTO getSingleEmpLeavestatus(LeaveCreditDTO data)
        {
            return COMMM.POSTDataOnlineLeave(data, "OnlineLeaveApplicationFacade/getSingleEmpLeavestatus/");
        }

        public LeaveCreditDTO deleterec(LeaveCreditDTO maspage)
        {
            return COMMM.POSTDataOnlineLeave(maspage, "OnlineLeaveApplicationFacade/deactivateRecordById/");
        }
        public LeaveCreditDTO requestleave(LeaveCreditDTO maspage)
        {
            return COMMM.POSTDataOnlineLeave(maspage, "OnlineLeaveApplicationFacade/requestleave/");
        }
        //--///////////////////////periodwiseleave//////////////////////
        public LeaveCreditDTO getdetails(LeaveCreditDTO data)
        {
            return COMMM.POSTDataOnlineLeave(data, "OnlineLeaveApplicationFacade/getdetails/");
        }
        public LeaveCreditDTO getabsentstaff(LeaveCreditDTO data)
        {
            return COMMM.POSTDataOnlineLeave(data, "OnlineLeaveApplicationFacade/getabsentstaff/");
        }
        public LeaveCreditDTO get_free_stfdets(LeaveCreditDTO data)
        {
            return COMMM.POSTDataOnlineLeave(data, "OnlineLeaveApplicationFacade/get_free_stfdets/");
        }
        public LeaveCreditDTO get_period_alloted(LeaveCreditDTO data)
        {
            return COMMM.POSTDataOnlineLeave(data, "OnlineLeaveApplicationFacade/get_period_alloted/");
        }
        public LeaveCreditDTO savedetails(LeaveCreditDTO data)
        {
            return COMMM.POSTDataOnlineLeave(data, "OnlineLeaveApplicationFacade/savedetails/");
        }
       public LeaveCreditDTO updatedetails(LeaveCreditDTO data)

        {
            return COMMM.POSTDataOnlineLeave(data, "OnlineLeaveApplicationFacade/updatedetails/");
        }
       
    }
}
