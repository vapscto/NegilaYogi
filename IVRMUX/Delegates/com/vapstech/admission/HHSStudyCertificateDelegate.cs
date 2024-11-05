using CommonLibrary;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.admission
{
    public class HHSStudyCertificateDelegate : CommonDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<HHSStudyCertificateDTO, HHSStudyCertificateDTO> _comm = new CommonDelegate<HHSStudyCertificateDTO, HHSStudyCertificateDTO>();
        public HHSStudyCertificateDTO getdetails(HHSStudyCertificateDTO id)
        {
            return _comm.POSTDataADM(id, "HHSStudyCertificateFacade/getdetails/");
        }
        public HHSStudyCertificateDTO GetStudDataById(HHSStudyCertificateDTO data)
        {
            return _comm.POSTDataADM(data, "HHSStudyCertificateFacade/getStudData/");
        }
        public HHSStudyCertificateDTO MigrationCertificateStuddetails(HHSStudyCertificateDTO data)
        {
            return _comm.POSTDataADM(data, "HHSStudyCertificateFacade/MigrationCertificateStuddetails/");
        }

        public HHSStudyCertificateDTO getstudlist(HHSStudyCertificateDTO student)
        {
            return _comm.POSTDataADM(student, "HHSStudyCertificateFacade/getS/");
        }
        public HHSStudyCertificateDTO onacademicyearchange(HHSStudyCertificateDTO data)
        {
            return _comm.POSTDataADM(data, "HHSStudyCertificateFacade/onacademicyearchange/");
        }
        public HHSStudyCertificateDTO searchfilter(HHSStudyCertificateDTO data)
        {
            return _comm.POSTDataADM(data, "HHSStudyCertificateFacade/searchfilter/");
        }
        public HHSStudyCertificateDTO getstudentname(HHSStudyCertificateDTO data)
        {
            return _comm.POSTDataADM(data, "HHSStudyCertificateFacade/getstudentname/");
        }

        // CertificateGeneratedReport
        public HHSStudyCertificateDTO CertificateGeneratedReportLoad(HHSStudyCertificateDTO data)
        {
            return _comm.POSTDataADM(data, "HHSStudyCertificateFacade/CertificateGeneratedReportLoad/");
        }
        public HHSStudyCertificateDTO GetCertificateGeneratedReport(HHSStudyCertificateDTO data)
        {
            return _comm.POSTDataADM(data, "HHSStudyCertificateFacade/GetCertificateGeneratedReport/");
        }
        
    }
}
