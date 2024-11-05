using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.interfaces
{
  public  interface TuitionFeeCertificateInterface
    {

        TuitionFeeCertificate_DTO getdata(TuitionFeeCertificate_DTO data);
        TuitionFeeCertificate_DTO searchfilter(TuitionFeeCertificate_DTO data);
        TuitionFeeCertificate_DTO onchangeyear(TuitionFeeCertificate_DTO data);
        TuitionFeeCertificate_DTO onchangeclass(TuitionFeeCertificate_DTO data);
        TuitionFeeCertificate_DTO onchangesection(TuitionFeeCertificate_DTO data);
        Task<TuitionFeeCertificate_DTO> getStudData(TuitionFeeCertificate_DTO stuDTO);
    }
}
