using CommonLibrary;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.admission
{
    public class JSHSAdmissionCertificateDelegate
    {
        CommonDelegate<JSHSAdmissionCertificate_DTO, JSHSAdmissionCertificate_DTO> comm = new CommonDelegate<JSHSAdmissionCertificate_DTO, JSHSAdmissionCertificate_DTO>();
        public JSHSAdmissionCertificate_DTO getdata(JSHSAdmissionCertificate_DTO data)
        {
            return comm.POSTDataADM(data, "JSHSAdmissionCertificateFacade/getdata");
        }
        public JSHSAdmissionCertificate_DTO searchfilter(JSHSAdmissionCertificate_DTO data)
        {
            return comm.POSTDataADM(data, "JSHSAdmissionCertificateFacade/searchfilter");
        }
        public JSHSAdmissionCertificate_DTO onchangeyear(JSHSAdmissionCertificate_DTO data)
        {
            return comm.POSTDataADM(data, "JSHSAdmissionCertificateFacade/onchangeyear");
        }
        public JSHSAdmissionCertificate_DTO onchangeclass(JSHSAdmissionCertificate_DTO data)
        {
            return comm.POSTDataADM(data, "JSHSAdmissionCertificateFacade/onchangeclass");
        }
        public JSHSAdmissionCertificate_DTO onchangesection(JSHSAdmissionCertificate_DTO data)
        {
            return comm.POSTDataADM(data, "JSHSAdmissionCertificateFacade/onchangesection");
        }
        public JSHSAdmissionCertificate_DTO getStudData(JSHSAdmissionCertificate_DTO stuDTO)
        {
            return comm.POSTDataADM(stuDTO, "JSHSAdmissionCertificateFacade/getStudData");
        }
    }
}
