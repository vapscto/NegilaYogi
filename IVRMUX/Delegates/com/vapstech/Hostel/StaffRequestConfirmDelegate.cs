using CommonLibrary;
using PreadmissionDTOs.com.vaps.Hostel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Hostel
{
    public class StaffRequestConfirmDelegate
    {
        CommonDelegate<StaffRequestConfirm_DTO, StaffRequestConfirm_DTO> _commnbranch = new CommonDelegate<StaffRequestConfirm_DTO, StaffRequestConfirm_DTO>();

        public StaffRequestConfirm_DTO loaddata(StaffRequestConfirm_DTO data)
        {
            return _commnbranch.Post_Hostel(data, "StaffRequestConfirmFacade/loaddata/");
        }
        public StaffRequestConfirm_DTO requestApproved(StaffRequestConfirm_DTO data)
        {
            return _commnbranch.Post_Hostel(data, "StaffRequestConfirmFacade/requestApproved/");
        }
        public StaffRequestConfirm_DTO requestRejected(StaffRequestConfirm_DTO data)
        {
            return _commnbranch.Post_Hostel(data, "StaffRequestConfirmFacade/requestRejected/");
        }
        
    }
}
