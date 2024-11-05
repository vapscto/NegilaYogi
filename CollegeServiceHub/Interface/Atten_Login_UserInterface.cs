using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Interface
{
    public interface Atten_Login_UserInterface
    {
        Atten_Login_UserDTO getalldetails(Atten_Login_UserDTO data);       
        Atten_Login_UserDTO get_courses(Atten_Login_UserDTO data);
        Atten_Login_UserDTO get_branches(Atten_Login_UserDTO data);
        Atten_Login_UserDTO get_semisters(Atten_Login_UserDTO data);
        Atten_Login_UserDTO savedata(Atten_Login_UserDTO data);
        Atten_Login_UserDTO view_subjects(Atten_Login_UserDTO data);
        Atten_Login_UserDTO Deletedetails(Atten_Login_UserDTO data);
    }
}
