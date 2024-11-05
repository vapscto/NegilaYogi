using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.HRMS
{
    public class masterLeavingReasonDelegate
    {
        CommonDelegate<masterLeavingReasonDTO, masterLeavingReasonDTO> comm = new CommonDelegate<masterLeavingReasonDTO, masterLeavingReasonDTO>();
        public masterLeavingReasonDTO loaddata(masterLeavingReasonDTO data)
        {
            return comm.POSTDataHRMS(data, "masterLeavingReasonFacade/loaddata");
        }
        public masterLeavingReasonDTO savedata(masterLeavingReasonDTO data)
        {
            return comm.POSTDataHRMS(data, "masterLeavingReasonFacade/savedata");
        }
        public masterLeavingReasonDTO EditData(masterLeavingReasonDTO data)
        {
            return comm.POSTDataHRMS(data, "masterLeavingReasonFacade/EditData");
        }
        public masterLeavingReasonDTO masterDecative(masterLeavingReasonDTO data)
        {
            return comm.POSTDataHRMS(data, "masterLeavingReasonFacade/masterDecative");
        }
    }
}
