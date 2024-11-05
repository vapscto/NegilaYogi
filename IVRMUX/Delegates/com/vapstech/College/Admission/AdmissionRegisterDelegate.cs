using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Admission
{
    public class AdmissionRegisterDelegate
    {
        CommonDelegate<AdmissionRegisterDTO, AdmissionRegisterDTO> _commbranch = new CommonDelegate<AdmissionRegisterDTO, AdmissionRegisterDTO>();
        public AdmissionRegisterDTO getdetails(AdmissionRegisterDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "AdmissionRegisterFacade/getdetails/");
        }
        public AdmissionRegisterDTO onselectAcdYear(AdmissionRegisterDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "AdmissionRegisterFacade/onselectAcdYear/");
        }
        public AdmissionRegisterDTO onselectCourse(AdmissionRegisterDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "AdmissionRegisterFacade/onselectCourse/");
        }
        public AdmissionRegisterDTO onselectBranch(AdmissionRegisterDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "AdmissionRegisterFacade/onselectBranch/");
        }
        public AdmissionRegisterDTO onreport(AdmissionRegisterDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "AdmissionRegisterFacade/onreport/");
        }
        public AdmissionRegisterDTO onreportnew(AdmissionRegisterDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "AdmissionRegisterFacade/onreportnew/");
        }

        
    }
}
