using CommonLibrary;
using PreadmissionDTOs.com.vaps.VisitorsManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VisitorsManagement
{
    public class StaffGatePassDelegate
    {
        CommonDelegate<StaffGatePass_DTO, StaffGatePass_DTO> COMSPRT = new CommonDelegate<StaffGatePass_DTO, StaffGatePass_DTO>();

        public StaffGatePass_DTO Getdetails(StaffGatePass_DTO data)
        {
            return COMSPRT.POSTDataVisitors(data, "StaffGatePassFacade/Getdetails/");
        }
        public StaffGatePass_DTO getdepchange(StaffGatePass_DTO obj)
        {
            return COMSPRT.POSTDataVisitors(obj, "StaffGatePassFacade/getdepchange/");
        }
        public StaffGatePass_DTO get_staff1(StaffGatePass_DTO obj)
        {
            return COMSPRT.POSTDataVisitors(obj, "StaffGatePassFacade/get_staff1/");
        }
        public StaffGatePass_DTO saverecord(StaffGatePass_DTO data)
        {
            return COMSPRT.POSTDataVisitors(data, "StaffGatePassFacade/saverecord/");
        }
        public StaffGatePass_DTO editrecord(StaffGatePass_DTO obj)
        {
            return COMSPRT.POSTDataVisitors(obj, "StaffGatePassFacade/editrecord/");
        }
        public StaffGatePass_DTO deactive(StaffGatePass_DTO obj)
        {
            return COMSPRT.POSTDataVisitors(obj, "StaffGatePassFacade/deactive/");
        }
        
        public StaffGatePass_DTO PrintGatePass(StaffGatePass_DTO obj)
        {
            return COMSPRT.POSTDataVisitors(obj, "StaffGatePassFacade/PrintGatePass/");
        }      
    }
}
