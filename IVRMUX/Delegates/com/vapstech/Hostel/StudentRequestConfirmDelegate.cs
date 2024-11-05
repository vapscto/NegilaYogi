using CommonLibrary;
using PreadmissionDTOs.com.vaps.Hostel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Hostel
{
    public class StudentRequestConfirmDelegate
    {
        CommonDelegate<StudentRequestConfirm_DTO, StudentRequestConfirm_DTO> _commnbranch = new CommonDelegate<StudentRequestConfirm_DTO, StudentRequestConfirm_DTO>();

        public StudentRequestConfirm_DTO loaddata(StudentRequestConfirm_DTO data)
        {
            return _commnbranch.Post_Hostel(data, "StudentRequestConfirmFacade/loaddata/");
        }
        public StudentRequestConfirm_DTO requestApproved(StudentRequestConfirm_DTO data)
        {
            return _commnbranch.Post_Hostel(data, "StudentRequestConfirmFacade/requestApproved/");
        }
        public StudentRequestConfirm_DTO requestRejected(StudentRequestConfirm_DTO data)
        {
            return _commnbranch.Post_Hostel(data, "StudentRequestConfirmFacade/requestRejected/");
        }
        public StudentRequestConfirm_DTO Ydeactive(StudentRequestConfirm_DTO data)
        {
            return _commnbranch.Post_Hostel(data, "StudentRequestConfirmFacade/Ydeactive/");
        }
    }
}
