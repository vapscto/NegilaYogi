using CommonLibrary;
using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Transport
{
    public class VahicalCertificateDelegate
    {
        CommonDelegate<VahicalCertificateDTO, VahicalCertificateDTO> _com = new CommonDelegate<VahicalCertificateDTO, VahicalCertificateDTO>();
        public VahicalCertificateDTO getdata(int id)
        {
            return _com.GetDataByIdTransport(id, "VahicalCertificateFacade/getdata/");
        }
        public VahicalCertificateDTO savedata(VahicalCertificateDTO data)
        {
            return _com.POSTDataTransport(data, "VahicalCertificateFacade/savedata/");
        }
        public VahicalCertificateDTO edit(VahicalCertificateDTO data)
        {
            return _com.POSTDataTransport(data, "VahicalCertificateFacade/edit/");
        }
        public VahicalCertificateDTO deleterecord(VahicalCertificateDTO data)
        {
            return _com.POSTDataTransport(data, "VahicalCertificateFacade/deleterecord/");
        }

        
        public VahicalCertificateDTO Onvahiclechange(VahicalCertificateDTO data)
        {
            return _com.POSTDataTransport(data, "VahicalCertificateFacade/Onvahiclechange/");
        }
        
    }
}
