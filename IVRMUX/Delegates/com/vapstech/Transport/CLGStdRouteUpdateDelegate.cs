using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.Transport;
using PreadmissionDTOs;
namespace corewebapi18072016.Delegates.com.vapstech.Transport
{
    public class CLGStdRouteUpdateDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<CLGStdRouteUpdateDTO, CLGStdRouteUpdateDTO> COMMM = new CommonDelegate<CLGStdRouteUpdateDTO, CLGStdRouteUpdateDTO>();
        CommonDelegate<PaymentDetails, PaymentDetails> pay = new CommonDelegate<PaymentDetails, PaymentDetails>();
        public CLGStdRouteUpdateDTO getloaddata(CLGStdRouteUpdateDTO data)
        {
            return COMMM.POSTDataTransport(data, "CLGStdRouteUpdateFacade/getloaddata/");
        }

        public CLGStdRouteUpdateDTO getloaddataintruction(CLGStdRouteUpdateDTO data)
        {
            return COMMM.POSTDataTransport(data, "CLGStdRouteUpdateFacade/getloaddataintruction/");
        }
        public CLGStdRouteUpdateDTO getstudata(CLGStdRouteUpdateDTO sddto)
        {
            return COMMM.POSTDataTransport(sddto, "CLGStdRouteUpdateFacade/getstudata/");
        }
        public CLGStdRouteUpdateDTO getbuspassdata(CLGStdRouteUpdateDTO sddto)
        {
            return COMMM.POSTDataTransport(sddto, "CLGStdRouteUpdateFacade/getbuspassdata/");
        }

        public CLGStdRouteUpdateDTO getbuspassdataupdate(CLGStdRouteUpdateDTO sddto)
        {
            return COMMM.POSTDataTransport(sddto, "CLGStdRouteUpdateFacade/getbuspassdataupdate/");
        }
        public CLGStdRouteUpdateDTO getroutedata(CLGStdRouteUpdateDTO sddto)
        {
            return COMMM.POSTDataTransport(sddto, "CLGStdRouteUpdateFacade/getroutedata/");
        }
        public CLGStdRouteUpdateDTO getlocationdata(CLGStdRouteUpdateDTO sddto)
        {
            return COMMM.POSTDataTransport(sddto, "CLGStdRouteUpdateFacade/getlocationdata/");
        }
        public CLGStdRouteUpdateDTO getlocationdataonly(CLGStdRouteUpdateDTO sddto)
        {
            return COMMM.POSTDataTransport(sddto, "CLGStdRouteUpdateFacade/getlocationdataonly/");
        }
        public CLGStdRouteUpdateDTO savedata(CLGStdRouteUpdateDTO student)
        {
            return COMMM.POSTDataTransport(student, "CLGStdRouteUpdateFacade/savedata/");
        }

        public CLGStdRouteUpdateDTO paynow(CLGStdRouteUpdateDTO dt)
        {

            return COMMM.POSTDataTransport(dt, "CLGStdRouteUpdateFacade/paynow/");

        }

        public PaymentDetails getpaymentresponse(PaymentDetails response)
        {

            return pay.POSTDataTransport(response, "CLGStdRouteUpdateFacade/getpaymentresponse/");

        }


        public CLGStdRouteUpdateDTO searchfilter(CLGStdRouteUpdateDTO sddto)
        {
            return COMMM.POSTDataTransport(sddto, "CLGStdRouteUpdateFacade/searchfilter/");
        }


    }
}
