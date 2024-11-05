using CommonLibrary;
using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Fees
{
    public class TuitionFeeCertificateDelegate
    {

        CommonDelegate<TuitionFeeCertificate_DTO, TuitionFeeCertificate_DTO> comm = new CommonDelegate<TuitionFeeCertificate_DTO, TuitionFeeCertificate_DTO>();
        public TuitionFeeCertificate_DTO getdata(TuitionFeeCertificate_DTO data)
        {
          
            return comm.POSTDatafee(data, "TuitionFeeCertificateFacade/getdata");
        }
        public TuitionFeeCertificate_DTO searchfilter(TuitionFeeCertificate_DTO data)
        {
            return comm.POSTDatafee(data, "TuitionFeeCertificateFacade/searchfilter");
          
        }
        public TuitionFeeCertificate_DTO onchangeyear(TuitionFeeCertificate_DTO data)
        {
            return comm.POSTDatafee(data, "TuitionFeeCertificateFacade/onchangeyear");
        }
        public TuitionFeeCertificate_DTO onchangeclass(TuitionFeeCertificate_DTO data)
        {
            return comm.POSTDatafee(data, "TuitionFeeCertificateFacade/onchangeclass");
        }
        public TuitionFeeCertificate_DTO onchangesection(TuitionFeeCertificate_DTO data)
        {
            return comm.POSTDatafee(data, "TuitionFeeCertificateFacade/onchangesection");
        }
        public TuitionFeeCertificate_DTO getStudData(TuitionFeeCertificate_DTO stuDTO)
        {
            return comm.POSTDatafee(stuDTO, "TuitionFeeCertificateFacade/getStudData");
        }
    }
}
