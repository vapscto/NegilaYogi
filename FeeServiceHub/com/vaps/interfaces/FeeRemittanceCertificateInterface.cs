using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;

namespace FeeServiceHub.com.vaps.interfaces
{
    public interface FeeRemittanceCertificateInterface
    {
        FeeRemittanceCertificateDTO getInitailData(int id);
        FeeRemittanceCertificateDTO SearchData(FeeRemittanceCertificateDTO Clscatag);
        FeeRemittanceCertificateDTO getAdm_Name(FeeRemittanceCertificateDTO Clscatag);
        
    }
}
