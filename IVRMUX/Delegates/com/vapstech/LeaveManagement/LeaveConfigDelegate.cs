using CommonLibrary;
using PreadmissionDTOs.com.vaps.LeaveManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.LeaveManagement
{
    public class LeaveConfigDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HR_Leave_Policy_Config_DTO, HR_Leave_Policy_Config_DTO> COMFRNT = new CommonDelegate<HR_Leave_Policy_Config_DTO, HR_Leave_Policy_Config_DTO>();

        public HR_Leave_Policy_Config_DTO savedata(HR_Leave_Policy_Config_DTO data)
        {
            return COMFRNT.POSTDataOnlineLeave(data, "LeaveConfigFacade/save/");
        }
        public HR_Leave_Policy_Config_DTO getSPName(HR_Leave_Policy_Config_DTO data)
        {
            return COMFRNT.POSTDataOnlineLeave(data, "LeaveConfigFacade/getSPName/");
        }
    }
}
