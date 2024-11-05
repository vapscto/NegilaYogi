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
    public class StudentBuspassFormDelegate
    {

        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
       
        CommonDelegate<StudentBuspassFormDTO, StudentBuspassFormDTO> COMMM = new CommonDelegate<StudentBuspassFormDTO, StudentBuspassFormDTO>();
        CommonDelegate<PaymentDetails, PaymentDetails> pay = new CommonDelegate<PaymentDetails, PaymentDetails>();
       
        public StudentBuspassFormDTO getloaddata(StudentBuspassFormDTO data)
        {
            return COMMM.POSTPORTALData(data, "StudentBuspassFormFacade/getloaddata/");
        }

        public StudentBuspassFormDTO getloaddataintruction(StudentBuspassFormDTO data)
        {
            return COMMM.POSTPORTALData(data, "StudentBuspassFormFacade/getloaddataintruction/");
        }
        public StudentBuspassFormDTO getstudata(StudentBuspassFormDTO sddto)
        {
            return COMMM.POSTPORTALData(sddto, "StudentBuspassFormFacade/getstudata/");
        }
        public StudentBuspassFormDTO getbuspassdata(StudentBuspassFormDTO sddto)
        {
            return COMMM.POSTPORTALData(sddto, "StudentBuspassFormFacade/getbuspassdata/");
        }
        public StudentBuspassFormDTO academicload(StudentBuspassFormDTO data)
        {
            return COMMM.POSTPORTALData(data, "StudentBuspassFormFacade/academicload/");
        }
        public StudentBuspassFormDTO getroutedata(StudentBuspassFormDTO sddto)
        {
            return COMMM.POSTPORTALData(sddto, "StudentBuspassFormFacade/getroutedata/");
        }
        public StudentBuspassFormDTO getlocationdata(StudentBuspassFormDTO sddto)
        {
            return COMMM.POSTPORTALData(sddto, "StudentBuspassFormFacade/getlocationdata/");
        }
        public StudentBuspassFormDTO getlocationdataonly(StudentBuspassFormDTO sddto)
        {
            return COMMM.POSTPORTALData(sddto, "StudentBuspassFormFacade/getlocationdataonly/");
        }
        public StudentBuspassFormDTO savedata(StudentBuspassFormDTO student)
        {
            return COMMM.POSTPORTALData(student, "StudentBuspassFormFacade/savedata/");
        }

        public StudentBuspassFormDTO paynow(StudentBuspassFormDTO dt)
        {

            return COMMM.POSTPORTALData(dt, "StudentBuspassFormFacade/paynow/");

        }
          public StudentBuspassFormDTO paynow1(StudentBuspassFormDTO dt)
        {

            return COMMM.POSTPORTALData(dt, "StudentBuspassFormFacade/paynow1/");

        }
        public StudentBuspassFormDTO paynow2(StudentBuspassFormDTO dt)
        {

            return COMMM.POSTPORTALData(dt, "StudentBuspassFormFacade/paynow2/");

        }

        public PaymentDetails getpaymentresponse(PaymentDetails response)
        {

            return pay.POSTPORTALData(response, "StudentBuspassFormFacade/getpaymentresponse/");

        }

        public PaymentDetails razorgetpaymentresponse(PaymentDetails response)
        {
            return pay.POSTPORTALData(response, "StudentBuspassFormFacade/Razorpaypaymentresponse/");
        }
        
    }
    }
