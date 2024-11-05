using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Transport;

namespace corewebapi18072016.Delegates.com.vapstech.Transport
{
    public class CLGStudentBuspassFormDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<CLGStudentBuspassFormDTO, CLGStudentBuspassFormDTO> COMMM = new CommonDelegate<CLGStudentBuspassFormDTO, CLGStudentBuspassFormDTO>();
        CommonDelegate<PaymentDetails, PaymentDetails> pay = new CommonDelegate<PaymentDetails, PaymentDetails>();

        public CLGStudentBuspassFormDTO getloaddata(CLGStudentBuspassFormDTO data)
        {
            return COMMM.POSTDataTransport(data, "CLGStudentBuspassFormFacade/getloaddata/");
        }

        public CLGStudentBuspassFormDTO getloaddataintruction(CLGStudentBuspassFormDTO data)
        {
            return COMMM.POSTDataTransport(data, "CLGStudentBuspassFormFacade/getloaddataintruction/");
        }
        public CLGStudentBuspassFormDTO getstudata(CLGStudentBuspassFormDTO sddto)
        {
            return COMMM.POSTDataTransport(sddto, "CLGStudentBuspassFormFacade/getstudata/");
        }
        public CLGStudentBuspassFormDTO getbuspassdata(CLGStudentBuspassFormDTO sddto)
        {
            return COMMM.POSTDataTransport(sddto, "CLGStudentBuspassFormFacade/getbuspassdata/");
        }
        public CLGStudentBuspassFormDTO academicload(CLGStudentBuspassFormDTO data)
        {
            return COMMM.POSTDataTransport(data, "CLGStudentBuspassFormFacade/academicload/");
        }
        public CLGStudentBuspassFormDTO getroutedata(CLGStudentBuspassFormDTO sddto)
        {
            return COMMM.POSTDataTransport(sddto, "CLGStudentBuspassFormFacade/getroutedata/");
        }
        public CLGStudentBuspassFormDTO getlocationdata(CLGStudentBuspassFormDTO sddto)
        {
            return COMMM.POSTDataTransport(sddto, "CLGStudentBuspassFormFacade/getlocationdata/");
        }
        public CLGStudentBuspassFormDTO getlocationdataonly(CLGStudentBuspassFormDTO sddto)
        {
            return COMMM.POSTDataTransport(sddto, "CLGStudentBuspassFormFacade/getlocationdataonly/");
        }
        public CLGStudentBuspassFormDTO savedata(CLGStudentBuspassFormDTO student)
        {
            return COMMM.POSTDataTransport(student, "CLGStudentBuspassFormFacade/savedata/");
        }

        public CLGStudentBuspassFormDTO paynow(CLGStudentBuspassFormDTO dt)
        {

            return COMMM.POSTDataTransport(dt, "CLGStudentBuspassFormFacade/paynow/");

        }
        public CLGStudentBuspassFormDTO paynow1(CLGStudentBuspassFormDTO dt)
        {

            return COMMM.POSTDataTransport(dt, "CLGStudentBuspassFormFacade/paynow1/");

        }
        public CLGStudentBuspassFormDTO paynow2(CLGStudentBuspassFormDTO dt)
        {

            return COMMM.POSTDataTransport(dt, "CLGStudentBuspassFormFacade/paynow2/");

        }

        public PaymentDetails getpaymentresponse(PaymentDetails response)
        {

            return pay.POSTDataTransport(response, "CLGStudentBuspassFormFacade/getpaymentresponse/");

        }

        public PaymentDetails razorgetpaymentresponse(PaymentDetails response)
        {
            return pay.POSTDataTransport(response, "CLGStudentBuspassFormFacade/Razorpaypaymentresponse/");
        }
    }

}
