using CommonLibrary;
using PreadmissionDTOs.com.vaps.LeaveManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.LeaveManagement
{
    public class AdminondutyapplyDelegate
    {
        CommonDelegate<AdminondutyapplyDTO, AdminondutyapplyDTO> COMMM = new CommonDelegate<AdminondutyapplyDTO, AdminondutyapplyDTO>();
        public AdminondutyapplyDTO GetData(AdminondutyapplyDTO DTO)
        {
            return COMMM.POSTDataOnlineLeave(DTO, "AdminondutyapplyFacade/GetData/");
        }
      
        public AdminondutyapplyDTO employeedetails(AdminondutyapplyDTO DTO)
        {
            return COMMM.POSTDataOnlineLeave(DTO, "AdminondutyapplyFacade/employeedetails/");
        }
        public AdminondutyapplyDTO requestleave(AdminondutyapplyDTO DTO)
        {
            return COMMM.POSTDataOnlineLeave(DTO, "AdminondutyapplyFacade/requestleave/");
        }
        public AdminondutyapplyDTO viewcomment(AdminondutyapplyDTO DTO)
        {
            return COMMM.POSTDataOnlineLeave(DTO, "AdminondutyapplyFacade/viewcomment/");
        }
        public AdminondutyapplyDTO ActiveDeactiveRecord(AdminondutyapplyDTO DTO)
        {
            return COMMM.POSTDataOnlineLeave(DTO, "AdminondutyapplyFacade/ActiveDeactiveRecord/");
        }
        public AdminondutyapplyDTO editData(AdminondutyapplyDTO DTO)
        {
            return COMMM.POSTDataOnlineLeave(DTO, "AdminondutyapplyFacade/editData/");
        }
    }

}
