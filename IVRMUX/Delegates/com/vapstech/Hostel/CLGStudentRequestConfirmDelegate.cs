using CommonLibrary;
using PreadmissionDTOs.com.vaps.Hostel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Hostel
{
    public class CLGStudentRequestConfirmDelegate
    {
        CommonDelegate<CLGStudentRequestConfirmDTO, CLGStudentRequestConfirmDTO> _commnbranch = new CommonDelegate<CLGStudentRequestConfirmDTO, CLGStudentRequestConfirmDTO>();

        public CLGStudentRequestConfirmDTO loaddata(CLGStudentRequestConfirmDTO data)
        {
            return _commnbranch.Post_Hostel(data, "CLGStudentRequestConfirmFacade/loaddata/");
        }
        public CLGStudentRequestConfirmDTO requestApproved(CLGStudentRequestConfirmDTO data)
        {
            return _commnbranch.Post_Hostel(data, "CLGStudentRequestConfirmFacade/requestApproved/");
        }
        public CLGStudentRequestConfirmDTO requestRejected(CLGStudentRequestConfirmDTO data)
        {
            return _commnbranch.Post_Hostel(data, "CLGStudentRequestConfirmFacade/requestRejected/");
        }
        public CLGStudentRequestConfirmDTO bedcapacity(CLGStudentRequestConfirmDTO data)
        {
            return _commnbranch.Post_Hostel(data, "CLGStudentRequestConfirmFacade/bedcapacity/");
        }
        public CLGStudentRequestConfirmDTO Ydeactive(CLGStudentRequestConfirmDTO data)
        {
            return _commnbranch.Post_Hostel(data, "CLGStudentRequestConfirmFacade/Ydeactive/");
        }
    }
}

