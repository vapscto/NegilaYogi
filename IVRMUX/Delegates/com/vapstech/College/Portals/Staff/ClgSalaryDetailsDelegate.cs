using System;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Portals.Staff;

namespace corewebapi18072016.Delegates.com.vapstech.College.Portals.Staff
{
    public class ClgSalaryDetailsDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ClgPortalHRMSDTO, ClgPortalHRMSDTO> COMMM = new CommonDelegate<ClgPortalHRMSDTO, ClgPortalHRMSDTO>();
        public ClgPortalHRMSDTO getloaddata(ClgPortalHRMSDTO data)
        {     
            return COMMM.CLGPortalPOSTData(data, "ClgSalaryDetailsFacade/getloaddata/");
        }
        public ClgPortalHRMSDTO getSalary(ClgPortalHRMSDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "ClgSalaryDetailsFacade/getSalary/");
        }
        public ClgPortalHRMSDTO getsalaryalldetails(ClgPortalHRMSDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "ClgSalaryDetailsFacade/getsalaryalldetails/");
        }

        
    }
}
