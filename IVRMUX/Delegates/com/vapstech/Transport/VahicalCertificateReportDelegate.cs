using CommonLibrary;
using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Transport
{
    public class VahicalCertificateReportDelegate
    {
        CommonDelegate<VahicalCertificateReportDTO, VahicalCertificateReportDTO> _com = new CommonDelegate<VahicalCertificateReportDTO, VahicalCertificateReportDTO>();
        public VahicalCertificateReportDTO getdata(int id)
        {
            return _com.GetDataByIdTransport(id, "VahicalCertificateReportFacade/getdata/");
        }
        public VahicalCertificateReportDTO savedata(VahicalCertificateReportDTO data)
        {
            return _com.POSTDataTransport(data, "VahicalCertificateReportFacade/savedata/");
        }
        public VahicalCertificateReportDTO edit(VahicalCertificateReportDTO data)
        {
            return _com.POSTDataTransport(data, "VahicalCertificateReportFacade/edit/");
        }
        public VahicalCertificateReportDTO deleterecord(VahicalCertificateReportDTO data)
        {
            return _com.POSTDataTransport(data, "VahicalCertificateReportFacade/deleterecord/");
        }

        
        public VahicalCertificateReportDTO Onvahiclechange(VahicalCertificateReportDTO data)
        {
            return _com.POSTDataTransport(data, "VahicalCertificateReportFacade/Onvahiclechange/");
        }
        
    }
}
