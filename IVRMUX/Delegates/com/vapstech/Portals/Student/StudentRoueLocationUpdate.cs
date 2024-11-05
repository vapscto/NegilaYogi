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

namespace corewebapi18072016.Delegates.com.vapstech.Portals.Student
{
    public class StudentRoueLocationUpdateDelegate 
    {

        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<StudentBuspassFormDTO, StudentBuspassFormDTO> COMMM = new CommonDelegate<StudentBuspassFormDTO, StudentBuspassFormDTO>();
        CommonDelegate<PaymentDetails, PaymentDetails> pay = new CommonDelegate<PaymentDetails, PaymentDetails>();
        public StudentBuspassFormDTO getloaddata(StudentBuspassFormDTO data)
        {
            return COMMM.POSTPORTALData(data, "StudentRoueLocationUpdateFacade/getloaddata/");
        }

        public StudentBuspassFormDTO getloaddataintruction(StudentBuspassFormDTO data)
        {
            return COMMM.POSTPORTALData(data, "StudentRoueLocationUpdateFacade/getloaddataintruction/");
        }
        public StudentBuspassFormDTO getstudata(StudentBuspassFormDTO sddto)
        {
            return COMMM.POSTPORTALData(sddto, "StudentRoueLocationUpdateFacade/getstudata/");
        }
        public StudentBuspassFormDTO getbuspassdata(StudentBuspassFormDTO sddto)
        {
            return COMMM.POSTPORTALData(sddto, "StudentRoueLocationUpdateFacade/getbuspassdata/");
        }

        public StudentBuspassFormDTO getbuspassdataupdate(StudentBuspassFormDTO sddto)
        {
            return COMMM.POSTPORTALData(sddto, "StudentRoueLocationUpdateFacade/getbuspassdataupdate/");
        }
        public StudentBuspassFormDTO getroutedata(StudentBuspassFormDTO sddto)
        {
            return COMMM.POSTPORTALData(sddto, "StudentRoueLocationUpdateFacade/getroutedata/");
        }
        public StudentBuspassFormDTO getlocationdata(StudentBuspassFormDTO sddto)
        {
            return COMMM.POSTPORTALData(sddto, "StudentRoueLocationUpdateFacade/getlocationdata/");
        }
        public StudentBuspassFormDTO getlocationdataonly(StudentBuspassFormDTO sddto)
        {
            return COMMM.POSTPORTALData(sddto, "StudentRoueLocationUpdateFacade/getlocationdataonly/");
        }
        public StudentBuspassFormDTO savedata(StudentBuspassFormDTO student)
        {
            return COMMM.POSTPORTALData(student, "StudentRoueLocationUpdateFacade/savedata/");
        }

        public StudentBuspassFormDTO paynow(StudentBuspassFormDTO dt)
        {

            return COMMM.POSTPORTALData(dt, "StudentRoueLocationUpdateFacade/paynow/");

        }

        public PaymentDetails getpaymentresponse(PaymentDetails response)
        {

            return pay.POSTPORTALData(response, "StudentRoueLocationUpdateFacade/getpaymentresponse/");

        }


        public StudentBuspassFormDTO searchfilter(StudentBuspassFormDTO sddto)
        {
            return COMMM.POSTPORTALData(sddto, "StudentRoueLocationUpdateFacade/searchfilter/");
        }



    }
    }
