using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.College.Admission
{
    public class Atten_Login_UserDelegate
    {
        CommonDelegate<Atten_Login_UserDTO, Atten_Login_UserDTO> _commbranch = new CommonDelegate<Atten_Login_UserDTO, Atten_Login_UserDTO>();

        public Atten_Login_UserDTO getalldetails(Atten_Login_UserDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "Atten_Login_UserFacade/getalldetails/");
        }
        public Atten_Login_UserDTO get_courses(Atten_Login_UserDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "Atten_Login_UserFacade/get_courses/");
        }
        public Atten_Login_UserDTO get_branches(Atten_Login_UserDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "Atten_Login_UserFacade/get_branches/");
        }
        public Atten_Login_UserDTO get_semisters(Atten_Login_UserDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "Atten_Login_UserFacade/get_semisters/");
        }           
        public Atten_Login_UserDTO savedata(Atten_Login_UserDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "Atten_Login_UserFacade/savedata/");
        }
        public Atten_Login_UserDTO view_subjects(Atten_Login_UserDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "Atten_Login_UserFacade/view_subjects/");
        }
        public Atten_Login_UserDTO Deletedetails(Atten_Login_UserDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "Atten_Login_UserFacade/Deletedetails/");
        }
    }
}
