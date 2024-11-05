using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.College.Admission
{
    public class StudentGeneralRegisterDelegates
    {
        CommonDelegate<StudentGeneralRegisterDTO, StudentGeneralRegisterDTO> _commbranch = new CommonDelegate<StudentGeneralRegisterDTO, StudentGeneralRegisterDTO>();
        public StudentGeneralRegisterDTO getdetails(StudentGeneralRegisterDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "StudentGeneralRegisterFacade/getdetails/");            
        }
        public StudentGeneralRegisterDTO onselectAcdYear(StudentGeneralRegisterDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "StudentGeneralRegisterFacade/onselectAcdYear/");            
        }
        public StudentGeneralRegisterDTO onselectCourse(StudentGeneralRegisterDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "StudentGeneralRegisterFacade/onselectCourse/");            
        }
        public StudentGeneralRegisterDTO onselectBranch(StudentGeneralRegisterDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "StudentGeneralRegisterFacade/onselectBranch/");            
        }
        public StudentGeneralRegisterDTO onreport(StudentGeneralRegisterDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "StudentGeneralRegisterFacade/onreport/");           
        }


    }
}
