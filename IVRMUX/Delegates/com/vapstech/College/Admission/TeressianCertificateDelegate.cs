using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Admission
{
    public class TeressianCertificateDelegate
    {
        CommonDelegate<TeressianCertificateDTO, TeressianCertificateDTO> _commbranch = new CommonDelegate<TeressianCertificateDTO, TeressianCertificateDTO>();
        public TeressianCertificateDTO getalldetails(TeressianCertificateDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "TeressianCertificateFacade/getdetails/");
        }
        public TeressianCertificateDTO getcoursedata(TeressianCertificateDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "TeressianCertificateFacade/getcoursedata/");
        }
        public TeressianCertificateDTO getbranchdata(TeressianCertificateDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "TeressianCertificateFacade/getbranchdata/");
        }
        public TeressianCertificateDTO getsemisterdata(TeressianCertificateDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "TeressianCertificateFacade/getsemisterdata/");
        }
        public TeressianCertificateDTO getsstudentdata(TeressianCertificateDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "TeressianCertificateFacade/getsstudentdata/");
        }
        public TeressianCertificateDTO GetCertificate(TeressianCertificateDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "TeressianCertificateFacade/GetCertificate/");
        }
     
    }
}
