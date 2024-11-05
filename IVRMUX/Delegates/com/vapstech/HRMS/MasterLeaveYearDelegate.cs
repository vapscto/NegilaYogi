using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class MasterLeaveYearDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HR_Master_LeaveYearDTO, HR_Master_LeaveYearDTO> COMMM = new CommonDelegate<HR_Master_LeaveYearDTO, HR_Master_LeaveYearDTO>();

        public HR_Master_LeaveYearDTO onloadgetdetails(HR_Master_LeaveYearDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "MasterLeaveYearFacade/onloadgetdetails");
        }

        public HR_Master_LeaveYearDTO savedetails(HR_Master_LeaveYearDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "MasterLeaveYearFacade/");
        }
        public HR_Master_LeaveYearDTO getRecorddetailsById(int id)
        {
            return COMMM.GetDataByIdHRMS(id, "MasterLeaveYearFacade/getRecordById/");
        }
        public HR_Master_LeaveYearDTO deleterec(HR_Master_LeaveYearDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "MasterLeaveYearFacade/deactivateRecordById/");
        }
        public HR_Master_LeaveYearDTO validateordernumber(HR_Master_LeaveYearDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "MasterLeaveYearFacade/validateordernumber/");
        }
    }
}
