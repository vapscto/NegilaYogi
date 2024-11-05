using CommonLibrary;
using PreadmissionDTOs.com.vaps.Hostel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Hostel
{
    public class HostelAllotAndConfermForCLGStudentDelegate
    {
        CommonDelegate<CLGStudentRequestConfirmDTO, CLGStudentRequestConfirmDTO> _commnbranch = new CommonDelegate<CLGStudentRequestConfirmDTO, CLGStudentRequestConfirmDTO>();

        public CLGStudentRequestConfirmDTO loaddata(CLGStudentRequestConfirmDTO data)
        {
            return _commnbranch.Post_Hostel(data, "HostelAllotAndConfermForCLGStudentFacade/loaddata/");
        }
        public CLGStudentRequestConfirmDTO requestApproved(CLGStudentRequestConfirmDTO data)
        {
            return _commnbranch.Post_Hostel(data, "HostelAllotAndConfermForCLGStudentFacade/requestApproved/");
        }
        public CLGStudentRequestConfirmDTO requestRejected(CLGStudentRequestConfirmDTO data)
        {
            return _commnbranch.Post_Hostel(data, "HostelAllotAndConfermForCLGStudentFacade/requestRejected/");
        }
        public CLGStudentRequestConfirmDTO bedcapacity(CLGStudentRequestConfirmDTO data)
        {
            return _commnbranch.Post_Hostel(data, "HostelAllotAndConfermForCLGStudentFacade/bedcapacity/");
        }
        public CLGStudentRequestConfirmDTO Ydeactive(CLGStudentRequestConfirmDTO data)
        {
            return _commnbranch.Post_Hostel(data, "HostelAllotAndConfermForCLGStudentFacade/Ydeactive/");
        }
        public CLGStudentRequestConfirmDTO get_studInfo(CLGStudentRequestConfirmDTO data)
        {
            return _commnbranch.Post_Hostel(data, "HostelAllotAndConfermForCLGStudentFacade/get_studInfo/");
        }
    }
}

