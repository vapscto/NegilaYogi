using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates
{
    public class BuspassFormDelegate
    {

        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<StudentHelthcertificateDTO, StudentHelthcertificateDTO> COMMM = new CommonDelegate<StudentHelthcertificateDTO, StudentHelthcertificateDTO>();
        CommonDelegate<PaymentDetails, PaymentDetails> pay = new CommonDelegate<PaymentDetails, PaymentDetails>();
        public StudentHelthcertificateDTO getloaddata(StudentHelthcertificateDTO data)
        {
            return COMMM.POSTData(data, "BuspassFormFacade/getloaddata/");
        }
        public StudentHelthcertificateDTO getstudata(StudentHelthcertificateDTO sddto)
        {
            return COMMM.POSTData(sddto, "BuspassFormFacade/getstudata/");
        }
        public StudentHelthcertificateDTO getbuspassdata(StudentHelthcertificateDTO sddto)
        {
            return COMMM.POSTData(sddto, "BuspassFormFacade/getbuspassdata/");
        }
        public StudentHelthcertificateDTO getroutedata(StudentHelthcertificateDTO sddto)
        {
            return COMMM.POSTData(sddto, "BuspassFormFacade/getroutedata/");
        }
        public StudentHelthcertificateDTO getlocationdata(StudentHelthcertificateDTO sddto)
        {
            return COMMM.POSTData(sddto, "BuspassFormFacade/getlocationdata/");
        }
        public StudentHelthcertificateDTO savedata(StudentHelthcertificateDTO student)
        {
            return COMMM.POSTData(student, "BuspassFormFacade/savedata/");
        }
        public StudentHelthcertificateDTO paynow(StudentHelthcertificateDTO dt)
        {

            return COMMM.POSTData(dt, "BuspassFormFacade/paynow/");

        }

        public PaymentDetails getpaymentresponse(PaymentDetails response)
        {

            return pay.POSTData(response, "BuspassFormFacade/getpaymentresponse/");

        }

    }

   
}
