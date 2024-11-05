using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransportServiceHub.Interfaces
{
   public  interface CLGStudentBuspassFormInterface
    {
        CLGStudentBuspassFormDTO getloaddata(CLGStudentBuspassFormDTO data);

        CLGStudentBuspassFormDTO getloaddataintruction(CLGStudentBuspassFormDTO data);
        CLGStudentBuspassFormDTO getstudata(CLGStudentBuspassFormDTO sddto);
        Task<CLGStudentBuspassFormDTO> getbuspassdata(CLGStudentBuspassFormDTO sddto);
        CLGStudentBuspassFormDTO getroutedata(CLGStudentBuspassFormDTO data);
        CLGStudentBuspassFormDTO academicload(CLGStudentBuspassFormDTO data);
        CLGStudentBuspassFormDTO getlocationdata(CLGStudentBuspassFormDTO data);
        CLGStudentBuspassFormDTO getlocationdataonly(CLGStudentBuspassFormDTO data);
      CLGStudentBuspassFormDTO savedata(CLGStudentBuspassFormDTO data);


        //CLGStudentBuspassFormDTO paynow(CLGStudentBuspassFormDTO data);
        //CLGStudentBuspassFormDTO paynow1(CLGStudentBuspassFormDTO data);
        //CLGStudentBuspassFormDTO paynow2(CLGStudentBuspassFormDTO data);

        //PaymentDetails payuresponse(PaymentDetails stu);
        //PaymentDetails Razorpaypaymentresponse(PaymentDetails stu);

    }
}
