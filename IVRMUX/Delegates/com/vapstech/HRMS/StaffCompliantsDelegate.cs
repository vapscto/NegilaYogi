using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;

namespace IVRMUX.Delegates.com.vapstech.HRMS
{
    public class StaffCompliantsDelegate
    {
        CommonDelegate<StaffCompliantsDTO, StaffCompliantsDTO> _comm = new CommonDelegate<StaffCompliantsDTO, StaffCompliantsDTO>();

        public StaffCompliantsDTO loaddata(StaffCompliantsDTO data)
        {
            return _comm.POSTDataHRMS(data, "StaffCompliantsFacade/loaddata");
        }
        public StaffCompliantsDTO OnChangeEmployee(StaffCompliantsDTO data)
        {
            return _comm.POSTDataHRMS(data, "StaffCompliantsFacade/OnChangeEmployee");
        }
        public StaffCompliantsDTO SaveDetails(StaffCompliantsDTO data)
        {
            return _comm.POSTDataHRMS(data, "StaffCompliantsFacade/SaveDetails");
        }
        public StaffCompliantsDTO EditDetails(StaffCompliantsDTO data)
        {
            return _comm.POSTDataHRMS(data, "StaffCompliantsFacade/EditDetails");
        }
        public StaffCompliantsDTO ActiveDeativeEmployeeCompliantsDetails(StaffCompliantsDTO data)
        {
            return _comm.POSTDataHRMS(data, "StaffCompliantsFacade/ActiveDeativeEmployeeCompliantsDetails");
        }
        public StaffCompliantsDTO GetReport(StaffCompliantsDTO data)
        {
            return _comm.POSTDataHRMS(data, "StaffCompliantsFacade/GetReport");
        }
        public StaffCompliantsDTO GetViewStaffLoaddata(StaffCompliantsDTO data)
        {
            return _comm.POSTDataHRMS(data, "StaffCompliantsFacade/GetViewStaffLoaddata");
        }
    }
}
